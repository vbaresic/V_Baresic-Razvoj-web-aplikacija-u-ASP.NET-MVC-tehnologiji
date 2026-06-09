/**
 * Autocomplete Dropdown Control
 * 
 * Provides AJAX-powered autocomplete functionality for input fields with dropdown results.
 * Supports single or multiple selections and custom result rendering.
 * 
 * Usage:
 * new AutocompleteDropdown({
 *     inputSelector: '#team-search',
 *     searchUrl: '/teams/search',
 *     minChars: 2,
 *     maxResults: 10,
 *     resultTemplate: (item) => item.name,
 *     onSelect: (item) => console.log(item)
 * });
 */

class AutocompleteDropdown {
    constructor(config = {}) {
        this.config = {
            minChars: config.minChars || 2,
            maxResults: config.maxResults || 10,
            debounceDelay: config.debounceDelay || 300,
            resultTemplate: config.resultTemplate || (item => item.text),
            onSelect: config.onSelect || (() => {}),
            onNoResults: config.onNoResults || (() => {}),
            ...config
        };

        this.inputElement = typeof config.inputSelector === 'string' 
            ? document.querySelector(config.inputSelector)
            : config.inputSelector;

        if (!this.inputElement) {
            console.error('AutocompleteDropdown: Input element not found');
            return;
        }

        this.dropdownContainer = null;
        this.dropdownList = null;
        this.selectedIndex = -1;
        this.results = [];
        this.debounceTimer = null;
        this.isOpen = false;
        this.currentRequest = null;

        this.init();
    }

    init() {
        this.createDropdown();
        this.attachEventListeners();
    }

    createDropdown() {
        // Create container
        this.dropdownContainer = document.createElement('div');
        this.dropdownContainer.className = 'autocomplete-dropdown-container';
        this.dropdownContainer.style.cssText = `
            position: absolute;
            top: 100%;
            left: 0;
            right: 0;
            display: none;
            background: white;
            border: 1px solid #ddd;
            border-top: none;
            max-height: 300px;
            overflow-y: auto;
            z-index: 1000;
            box-shadow: 0 4px 6px rgba(0,0,0,0.1);
        `;

        // Create list
        this.dropdownList = document.createElement('ul');
        this.dropdownList.className = 'autocomplete-dropdown-list';
        this.dropdownList.style.cssText = `
            list-style: none;
            margin: 0;
            padding: 0;
        `;

        this.dropdownContainer.appendChild(this.dropdownList);

        // Insert after input element
        this.inputElement.parentNode.style.position = 'relative';
        this.inputElement.parentNode.insertBefore(
            this.dropdownContainer,
            this.inputElement.nextSibling
        );
    }

    attachEventListeners() {
        this.inputElement.addEventListener('input', (e) => this.handleInput(e));
        this.inputElement.addEventListener('keydown', (e) => this.handleKeydown(e));
        this.inputElement.addEventListener('blur', () => this.close());
        document.addEventListener('click', (e) => {
            if (!this.dropdownContainer.contains(e.target) && e.target !== this.inputElement) {
                this.close();
            }
        });
    }

    handleInput(e) {
        const query = e.target.value.trim();

        clearTimeout(this.debounceTimer);

        if (query.length < this.config.minChars) {
            this.close();
            return;
        }

        // Cancel previous request
        if (this.currentRequest) {
            this.currentRequest.abort?.();
        }

        this.debounceTimer = setTimeout(() => this.search(query), this.config.debounceDelay);
    }

    handleKeydown(e) {
        if (!this.isOpen) return;

        switch (e.key) {
            case 'ArrowDown':
                e.preventDefault();
                this.selectNext();
                break;
            case 'ArrowUp':
                e.preventDefault();
                this.selectPrevious();
                break;
            case 'Enter':
                e.preventDefault();
                if (this.selectedIndex >= 0 && this.results[this.selectedIndex]) {
                    this.selectItem(this.results[this.selectedIndex]);
                }
                break;
            case 'Escape':
                e.preventDefault();
                this.close();
                break;
        }
    }

    async search(query) {
        try {
            const url = new URL(this.config.searchUrl, window.location.origin);
            url.searchParams.append('query', query);

            const controller = new AbortController();
            this.currentRequest = controller;

            const response = await fetch(url.toString(), {
                signal: controller.signal
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            this.results = await response.json();

            if (this.results.length === 0) {
                this.config.onNoResults();
                this.showNoResults();
            } else {
                this.render(this.results.slice(0, this.config.maxResults));
                this.open();
            }
        } catch (error) {
            if (error.name !== 'AbortError') {
                console.error('AutocompleteDropdown: Search error', error);
                this.showError();
            }
        }
    }

    render(results) {
        this.dropdownList.innerHTML = '';
        this.selectedIndex = -1;

        results.forEach((result, index) => {
            const li = document.createElement('li');
            li.className = 'autocomplete-dropdown-item';
            li.style.cssText = `
                padding: 10px 15px;
                cursor: pointer;
                border-bottom: 1px solid #f0f0f0;
                transition: background 0.15s;
            `;
            li.textContent = this.config.resultTemplate(result);
            li.dataset.index = index;

            li.addEventListener('mouseenter', () => {
                this.clearSelection();
                this.selectedIndex = index;
                li.style.background = '#f5f5f5';
            });

            li.addEventListener('click', () => {
                this.selectItem(result);
            });

            this.dropdownList.appendChild(li);
        });
    }

    selectNext() {
        const items = this.dropdownList.querySelectorAll('li');
        this.clearSelection();
        this.selectedIndex = Math.min(this.selectedIndex + 1, items.length - 1);
        this.highlightSelected();
    }

    selectPrevious() {
        this.clearSelection();
        this.selectedIndex = Math.max(this.selectedIndex - 1, 0);
        this.highlightSelected();
    }

    clearSelection() {
        const items = this.dropdownList.querySelectorAll('li');
        items.forEach(item => item.style.background = '');
    }

    highlightSelected() {
        const items = this.dropdownList.querySelectorAll('li');
        if (items[this.selectedIndex]) {
            items[this.selectedIndex].style.background = '#f5f5f5';
        }
    }

    selectItem(result) {
        this.config.onSelect(result);
        this.inputElement.value = '';
        this.close();
    }

    showNoResults() {
        this.dropdownList.innerHTML = `
            <li style="padding: 15px; text-align: center; color: #999;">
                No results found
            </li>
        `;
        this.open();
    }

    showError() {
        this.dropdownList.innerHTML = `
            <li style="padding: 15px; text-align: center; color: #d9534f;">
                Error loading results
            </li>
        `;
        this.open();
    }

    open() {
        this.dropdownContainer.style.display = 'block';
        this.isOpen = true;
    }

    close() {
        this.dropdownContainer.style.display = 'none';
        this.isOpen = false;
        this.selectedIndex = -1;
    }

    destroy() {
        clearTimeout(this.debounceTimer);
        this.dropdownContainer.remove();
        this.inputElement.removeEventListener('input', this.handleInput);
        this.inputElement.removeEventListener('keydown', this.handleKeydown);
        this.inputElement.removeEventListener('blur', () => this.close());
    }
}

// Export for use in modules
if (typeof module !== 'undefined' && module.exports) {
    module.exports = AutocompleteDropdown;
}
