/**
 * UI Animations Module
 * 
 * Provides smooth animations for common UI interactions:
 * - Form field focus states with smooth underline
 * - Loading spinners and skeleton screens
 * - Fade in/out transitions
 * - Slide up/down animations
 * - Pulse animations for CTAs
 * - Bounce effects
 * 
 * Automatically initializes on DOMContentLoaded.
 */

const UIAnimations = {
    /**
     * Initialize all animations on page load
     */
    init() {
        this.setupFormAnimations();
        this.setupLoadingStates();
        this.setupTransitions();
        this.injectStylesheet();
    },

    /**
     * Inject CSS animations into the document
     */
    injectStylesheet() {
        if (document.querySelector('style[data-ui-animations]')) {
            return; // Already injected
        }

        const style = document.createElement('style');
        style.setAttribute('data-ui-animations', 'true');
        style.textContent = `
            /* Form field animations */
            .form-input,
            .form-select,
            textarea {
                transition: border-color 0.3s ease, box-shadow 0.3s ease;
            }

            .form-input:focus,
            .form-select:focus,
            textarea:focus {
                border-color: var(--accent, #007bff);
                box-shadow: 0 0 0 3px rgba(0, 123, 255, 0.15);
                outline: none;
                animation: focusPulse 0.4s ease-out;
            }

            @keyframes focusPulse {
                0% {
                    box-shadow: 0 0 0 0 rgba(0, 123, 255, 0.4);
                }
                100% {
                    box-shadow: 0 0 0 3px rgba(0, 123, 255, 0.15);
                }
            }

            /* Loading spinner */
            .spinner {
                display: inline-block;
                width: 20px;
                height: 20px;
                border: 3px solid rgba(0, 0, 0, 0.1);
                border-radius: 50%;
                border-top-color: var(--accent, #007bff);
                animation: spin 0.8s linear infinite;
            }

            .spinner--large {
                width: 40px;
                height: 40px;
                border-width: 4px;
            }

            @keyframes spin {
                to {
                    transform: rotate(360deg);
                }
            }

            /* Fade animations */
            .fade-in {
                animation: fadeIn 0.4s ease-in;
            }

            .fade-out {
                animation: fadeOut 0.4s ease-out;
                opacity: 0;
            }

            @keyframes fadeIn {
                from {
                    opacity: 0;
                }
                to {
                    opacity: 1;
                }
            }

            @keyframes fadeOut {
                from {
                    opacity: 1;
                }
                to {
                    opacity: 0;
                }
            }

            /* Slide animations */
            .slide-up {
                animation: slideUp 0.4s ease-out;
            }

            .slide-down {
                animation: slideDown 0.4s ease-out;
            }

            @keyframes slideUp {
                from {
                    transform: translateY(20px);
                    opacity: 0;
                }
                to {
                    transform: translateY(0);
                    opacity: 1;
                }
            }

            @keyframes slideDown {
                from {
                    transform: translateY(-20px);
                    opacity: 0;
                }
                to {
                    transform: translateY(0);
                    opacity: 1;
                }
            }

            /* Pulse animation for CTAs */
            .pulse {
                animation: pulse 2s ease-in-out infinite;
            }

            @keyframes pulse {
                0%, 100% {
                    transform: scale(1);
                }
                50% {
                    transform: scale(1.05);
                }
            }

            /* Bounce animation */
            .bounce {
                animation: bounce 0.6s ease-out;
            }

            @keyframes bounce {
                0%, 100% {
                    transform: translateY(0);
                }
                50% {
                    transform: translateY(-10px);
                }
            }

            /* Skeleton screen loading */
            .skeleton {
                background: linear-gradient(
                    90deg,
                    rgba(0, 0, 0, 0.1) 0%,
                    rgba(0, 0, 0, 0.05) 50%,
                    rgba(0, 0, 0, 0.1) 100%
                );
                background-size: 200% 100%;
                animation: skeletonLoading 1.5s ease-in-out infinite;
            }

            @keyframes skeletonLoading {
                0% {
                    background-position: 200% 0;
                }
                100% {
                    background-position: -200% 0;
                }
            }

            /* Smooth transitions */
            * {
                transition: background-color 0.2s ease, color 0.2s ease;
            }

            /* Button hover animations */
            .btn {
                transition: transform 0.2s ease, box-shadow 0.2s ease;
                position: relative;
                overflow: hidden;
            }

            .btn:hover:not(:disabled) {
                transform: translateY(-2px);
                box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
            }

            .btn:active:not(:disabled) {
                transform: translateY(0);
            }

            /* Button ripple effect on click */
            .btn::before {
                content: '';
                position: absolute;
                top: 50%;
                left: 50%;
                width: 0;
                height: 0;
                border-radius: 50%;
                background: rgba(255, 255, 255, 0.5);
                transform: translate(-50%, -50%);
                pointer-events: none;
            }

            .btn.ripple::before {
                animation: ripple 0.6s ease-out;
            }

            @keyframes ripple {
                to {
                    width: 300px;
                    height: 300px;
                    opacity: 0;
                }
            }

            /* Form validation animations */
            .form-error {
                animation: slideDown 0.3s ease-out;
                color: var(--danger, #d9534f);
                font-size: 0.875rem;
                margin-top: 0.25rem;
            }

            .form-success {
                animation: slideUp 0.3s ease-out;
                color: var(--success, #5cb85c);
                font-size: 0.875rem;
                margin-top: 0.25rem;
            }

            /* Search results animations */
            .search-result-item {
                animation: slideUp 0.3s ease-out;
            }

            .search-result-item:nth-child(1) { animation-delay: 0.05s; }
            .search-result-item:nth-child(2) { animation-delay: 0.1s; }
            .search-result-item:nth-child(3) { animation-delay: 0.15s; }
            .search-result-item:nth-child(n+4) { animation-delay: 0.2s; }

            /* Modal/Overlay animations */
            .modal-open {
                animation: fadeIn 0.3s ease-out;
            }

            .modal-close {
                animation: fadeOut 0.3s ease-out;
            }

            /* Page transition animations */
            .page-transition-enter {
                animation: fadeIn 0.3s ease-in;
            }

            .page-transition-exit {
                animation: fadeOut 0.3s ease-out;
            }

            /* Scale animations */
            .scale-in {
                animation: scaleIn 0.3s ease-out;
            }

            @keyframes scaleIn {
                from {
                    transform: scale(0.95);
                    opacity: 0;
                }
                to {
                    transform: scale(1);
                    opacity: 1;
                }
            }
        `;

        document.head.appendChild(style);
    },

    /**
     * Setup form field animations
     */
    setupFormAnimations() {
        const formInputs = document.querySelectorAll('.form-input, .form-select, textarea');

        formInputs.forEach(input => {
            input.addEventListener('focus', function() {
                this.classList.add('focused');
            });

            input.addEventListener('blur', function() {
                this.classList.remove('focused');
            });

            // Animate label if using floating labels
            const label = this.findAssociatedLabel(input);
            if (label) {
                input.addEventListener('focus', () => {
                    label.classList.add('label-active');
                });

                input.addEventListener('blur', () => {
                    if (!input.value) {
                        label.classList.remove('label-active');
                    }
                });
            }
        });
    },

    /**
     * Setup loading state animations
     */
    setupLoadingStates() {
        // Create a global function for showing loading state
        window.showLoadingState = (element) => {
            element.classList.add('loading');
            const spinner = document.createElement('div');
            spinner.className = 'spinner';
            element.appendChild(spinner);
        };

        window.hideLoadingState = (element) => {
            element.classList.remove('loading');
            const spinner = element.querySelector('.spinner');
            if (spinner) spinner.remove();
        };

        // Auto-show loading state on form submit
        document.addEventListener('submit', (e) => {
            const form = e.target;
            const submitBtn = form.querySelector('button[type="submit"]');
            if (submitBtn) {
                submitBtn.classList.add('loading');
            }
        });
    },

    /**
     * Setup smooth transitions
     */
    setupTransitions() {
        // Add fade-in animation to dynamically added content
        const observer = new MutationObserver((mutations) => {
            mutations.forEach((mutation) => {
                mutation.addedNodes.forEach((node) => {
                    if (node.nodeType === 1 && node.classList) { // Element node
                        node.classList.add('fade-in');
                    }
                });
            });
        });

        observer.observe(document.body, {
            childList: true,
            subtree: true
        });
    },

    /**
     * Utility: Find label associated with input
     */
    findAssociatedLabel(input) {
        if (input.id) {
            return document.querySelector(`label[for="${input.id}"]`);
        }
        return input.closest('label');
    },

    /**
     * Add ripple effect to button click
     */
    addRippleEffect(button) {
        button.addEventListener('click', function(e) {
            this.classList.remove('ripple');
            void this.offsetWidth; // Trigger reflow
            this.classList.add('ripple');

            setTimeout(() => {
                this.classList.remove('ripple');
            }, 600);
        });
    },

    /**
     * Animate element entrance
     */
    animateIn(element, animation = 'fadeIn', duration = 400) {
        element.style.animation = `${animation} ${duration}ms ease-out`;
    },

    /**
     * Animate element exit
     */
    animateOut(element, animation = 'fadeOut', duration = 400) {
        return new Promise(resolve => {
            element.style.animation = `${animation} ${duration}ms ease-out`;
            setTimeout(() => {
                resolve();
            }, duration);
        });
    }
};

// Auto-initialize when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => UIAnimations.init());
} else {
    UIAnimations.init();
}

// Export for use in modules
if (typeof module !== 'undefined' && module.exports) {
    module.exports = UIAnimations;
}
