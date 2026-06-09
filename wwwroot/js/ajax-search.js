/**
 * AJAX Search Module
 * Handles asynchronous search on list pages
 */

function initializeAjaxSearch(config) {
    const {
        searchUrl,
        resultGrid,
        searchInput,
        searchBtn,
        clearBtn,
        searchMessage,
        cardTemplate
    } = config;

    const inputEl = document.querySelector(searchInput);
    const btnEl = document.querySelector(searchBtn);
    const clearBtnEl = document.querySelector(clearBtn);
    const gridEl = document.querySelector(resultGrid);
    const messageEl = document.querySelector(searchMessage);
    let originalContent = gridEl.innerHTML;

    // Search on Enter key
    inputEl.addEventListener('keypress', (e) => {
        if (e.key === 'Enter') {
            e.preventDefault();
            performSearch();
        }
    });

    // Search on button click
    btnEl.addEventListener('click', performSearch);

    // Real-time search as user types (debounced)
    let searchTimeout;
    inputEl.addEventListener('input', () => {
        clearTimeout(searchTimeout);
        const query = inputEl.value.trim();
        
        if (query.length === 0) {
            clearSearch();
            return;
        }

        if (query.length >= 2) {
            searchTimeout = setTimeout(performSearch, 300);
        }
    });

    // Clear search
    clearBtnEl.addEventListener('click', clearSearch);

    async function performSearch() {
        const query = inputEl.value.trim();

        if (!query) {
            messageEl.textContent = 'Please enter a search term';
            messageEl.className = 'search-message error';
            return;
        }

        try {
            messageEl.textContent = 'Searching...';
            messageEl.className = 'search-message loading';
            btnEl.disabled = true;

            const response = await fetch(`${searchUrl}?query=${encodeURIComponent(query)}`);
            
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const results = await response.json();
            
            if (results.length === 0) {
                messageEl.textContent = `No results found for "${query}"`;
                messageEl.className = 'search-message info';
                gridEl.innerHTML = '<p class="no-results">No coaches match your search.</p>';
            } else {
                messageEl.textContent = `Found ${results.length} result${results.length !== 1 ? 's' : ''}`;
                messageEl.className = 'search-message success';
                
                gridEl.innerHTML = results
                    .map((card, index) => cardTemplate(card, index))
                    .join('');
            }

            clearBtnEl.style.display = 'block';
        } catch (error) {
            console.error('Search error:', error);
            messageEl.textContent = 'An error occurred during search. Please try again.';
            messageEl.className = 'search-message error';
        } finally {
            btnEl.disabled = false;
        }
    }

    function clearSearch() {
        inputEl.value = '';
        gridEl.innerHTML = originalContent;
        messageEl.textContent = '';
        messageEl.className = '';
        clearBtnEl.style.display = 'none';
        inputEl.focus();
    }
}
