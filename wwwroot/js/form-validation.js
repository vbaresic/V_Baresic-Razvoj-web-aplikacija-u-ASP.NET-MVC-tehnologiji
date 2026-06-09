/**
 * Form Validation Module
 * Handles client-side validation on blur and form submission
 */

document.addEventListener('DOMContentLoaded', function() {
    const forms = document.querySelectorAll('.ajax-form');
    
    forms.forEach(form => {
        initializeFormValidation(form);
    });

    function initializeFormValidation(form) {
        const inputs = form.querySelectorAll('input, textarea, select');
        
        // Validate on blur (when field loses focus)
        inputs.forEach(input => {
            input.addEventListener('blur', function() {
                validateField(this);
            });

            // Real-time validation as user types
            input.addEventListener('input', function() {
                if (this.classList.contains('is-invalid')) {
                    validateField(this);
                }
            });
        });

        // Form submission validation
        form.addEventListener('submit', function(e) {
            let isValid = true;

            inputs.forEach(input => {
                if (!validateField(input)) {
                    isValid = false;
                }
            });

            if (!isValid) {
                e.preventDefault();
                showFormError(form, 'Please fix the errors below before submitting.');
            }
        });
    }

    function validateField(field) {
        const errorElement = field.nextElementSibling;
        if (!errorElement || !errorElement.classList.contains('form-error')) {
            return true;
        }

        let isValid = true;
        let errorMessage = '';

        // Remove previous error styling
        field.classList.remove('is-invalid');
        errorElement.textContent = '';

        // Required field validation
        if (field.hasAttribute('required') && !field.value.trim()) {
            isValid = false;
            errorMessage = `${getFieldLabel(field)} is required`;
        }
        // Email validation
        else if (field.type === 'email' && field.value && !isValidEmail(field.value)) {
            isValid = false;
            errorMessage = 'Please enter a valid email address';
        }
        // Number validation
        else if (field.type === 'number') {
            if (field.hasAttribute('min') && Number(field.value) < Number(field.getAttribute('min'))) {
                isValid = false;
                errorMessage = `Value must be at least ${field.getAttribute('min')}`;
            }
            if (field.hasAttribute('max') && Number(field.value) > Number(field.getAttribute('max'))) {
                isValid = false;
                errorMessage = `Value must not exceed ${field.getAttribute('max')}`;
            }
        }
        // Date validation
        else if (field.type === 'date' || field.type === 'datetime-local') {
            if (field.value) {
                const date = new Date(field.value);
                if (isNaN(date.getTime())) {
                    isValid = false;
                    errorMessage = 'Please enter a valid date';
                }
            }
        }
        // Min length validation
        else if (field.hasAttribute('minlength') && field.value.length > 0) {
            const minLength = Number(field.getAttribute('minlength'));
            if (field.value.length < minLength) {
                isValid = false;
                errorMessage = `Minimum ${minLength} characters required`;
            }
        }
        // Max length validation
        else if (field.hasAttribute('maxlength') && field.value.length > 0) {
            const maxLength = Number(field.getAttribute('maxlength'));
            if (field.value.length > maxLength) {
                isValid = false;
                errorMessage = `Maximum ${maxLength} characters allowed`;
            }
        }
        // Pattern validation
        else if (field.hasAttribute('pattern') && field.value) {
            const pattern = new RegExp(field.getAttribute('pattern'));
            if (!pattern.test(field.value)) {
                isValid = false;
                errorMessage = `Invalid format for ${getFieldLabel(field)}`;
            }
        }

        if (!isValid) {
            field.classList.add('is-invalid');
            errorElement.textContent = errorMessage;
            errorElement.style.display = 'block';
        } else {
            field.classList.remove('is-invalid');
            errorElement.style.display = 'none';
        }

        return isValid;
    }

    function getFieldLabel(field) {
        const label = field.previousElementSibling;
        if (label && label.classList.contains('form-label')) {
            return label.textContent.trim();
        }
        return field.name || field.id || 'This field';
    }

    function isValidEmail(email) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    }

    function showFormError(form, message) {
        let alertBox = form.querySelector('.alert-danger');
        if (!alertBox) {
            alertBox = document.createElement('div');
            alertBox.className = 'alert alert-danger';
            form.insertBefore(alertBox, form.firstChild);
        }
        alertBox.textContent = message;
        alertBox.style.display = 'block';
        alertBox.scrollIntoView({ behavior: 'smooth', block: 'center' });
    }
});
