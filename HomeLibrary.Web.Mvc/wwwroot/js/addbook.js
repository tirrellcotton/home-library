/**
 * AddBook Module
 * Handles form interactions, cover image preview, and client-side validation for the Add Book page
 */
class AddBook {
    constructor() {
        this.debounceTimer = null;
        this.validationTimers = new Map();
        this.elements = {
            coverUrlInput: document.querySelector('textarea[name="CoverImageUrl"]'),
            coverPreview: document.getElementById('coverPreview'),
            form: document.querySelector('.add-book-form'),
            titleInput: document.querySelector('input[name="Title"]'),
            genreSelect: document.querySelector('select[name="GenreId"]'),
            isbnInput: document.querySelector('input[name="Isbn"]'),
            publishedYearInput: document.querySelector('input[name="PublishedYear"]'),
            bookStatusSelect: document.querySelector('select[name="BookStatusId"]'),
            notesTextarea: document.querySelector('textarea[name="Notes"]')
        };
        this.init();
    }
    init() {
        this.attachEventListeners();
        this.checkInitialCoverUrl();
    }
    attachEventListeners() {
        // Cover URL input with debounce
        this.elements.coverUrlInput?.addEventListener('input', () => this.handleCoverUrlInput());
        // Form submission
        this.elements.form?.addEventListener('submit', (e) => this.handleFormSubmit(e));
        // Real-time validation on blur and input
        this.elements.titleInput?.addEventListener('blur', () => this.validateTitle());
        this.elements.titleInput?.addEventListener('input', () => this.debouncedValidate('title', () => this.validateTitle()));
        this.elements.genreSelect?.addEventListener('change', () => this.validateGenre());
        this.elements.isbnInput?.addEventListener('blur', () => this.validateIsbn());
        this.elements.isbnInput?.addEventListener('input', () => this.debouncedValidate('isbn', () => this.validateIsbn()));
        this.elements.publishedYearInput?.addEventListener('blur', () => this.validatePublishedYear());
        this.elements.publishedYearInput?.addEventListener('input', () => this.debouncedValidate('year', () => this.validatePublishedYear()));
        this.elements.bookStatusSelect?.addEventListener('change', () => this.validateBookStatus());
        this.elements.notesTextarea?.addEventListener('blur', () => this.validateNotes());
        this.elements.notesTextarea?.addEventListener('input', () => this.debouncedValidate('notes', () => this.validateNotes()));
        this.elements.coverUrlInput?.addEventListener('blur', () => this.validateCoverUrl());
    }
    debouncedValidate(key, validationFn) {
        const existingTimer = this.validationTimers.get(key);
        if (existingTimer) {
            window.clearTimeout(existingTimer);
        }
        const timer = window.setTimeout(() => {
            validationFn();
            this.validationTimers.delete(key);
        }, 500);
        this.validationTimers.set(key, timer);
    }
    validateTitle() {
        const input = this.elements.titleInput;
        if (!input)
            return true;
        const value = input.value.trim();
        if (!value) {
            this.showError(input, 'Title is required');
            return false;
        }
        if (value.length > 200) {
            this.showError(input, 'Title cannot exceed 200 characters');
            return false;
        }
        this.clearError(input);
        return true;
    }
    validateGenre() {
        const select = this.elements.genreSelect;
        if (!select)
            return true;
        const value = select.value;
        if (!value || value === '') {
            this.showError(select, 'Genre is required');
            return false;
        }
        this.clearError(select);
        return true;
    }
    validateIsbn() {
        const input = this.elements.isbnInput;
        if (!input)
            return true;
        const value = input.value.trim();
        // ISBN is optional, but if provided, validate length
        if (value && value.length > 50) {
            this.showError(input, 'ISBN cannot exceed 50 characters');
            return false;
        }
        // Optional: Basic ISBN format validation (ISBN-10 or ISBN-13)
        if (value) {
            const cleanIsbn = value.replace(/[-\s]/g, '');
            if (cleanIsbn.length !== 10 && cleanIsbn.length !== 13) {
                this.showError(input, 'ISBN must be 10 or 13 digits');
                return false;
            }
            if (!/^\d+$/.test(cleanIsbn)) {
                this.showError(input, 'ISBN must contain only numbers');
                return false;
            }
        }
        this.clearError(input);
        return true;
    }
    validatePublishedYear() {
        const input = this.elements.publishedYearInput;
        if (!input)
            return true;
        const value = input.value.trim();
        // Published year is optional
        if (!value) {
            this.clearError(input);
            return true;
        }
        const year = parseInt(value, 10);
        if (isNaN(year)) {
            this.showError(input, 'Please enter a valid year');
            return false;
        }
        if (year < 1000 || year > 9999) {
            this.showError(input, 'Please enter a valid year (1000-9999)');
            return false;
        }
        const currentYear = new Date().getFullYear();
        if (year > currentYear + 1) {
            this.showError(input, `Year cannot be greater than ${currentYear + 1}`);
            return false;
        }
        this.clearError(input);
        return true;
    }
    validateBookStatus() {
        const select = this.elements.bookStatusSelect;
        if (!select)
            return true;
        const value = select.value;
        if (!value || value === '') {
            this.showError(select, 'Status is required');
            return false;
        }
        this.clearError(select);
        return true;
    }
    validateCoverUrl() {
        const input = this.elements.coverUrlInput;
        if (!input)
            return true;
        const value = input.value.trim();
        // Cover URL is optional
        if (!value) {
            this.clearError(input);
            return true;
        }
        if (!this.isValidUrl(value)) {
            this.showError(input, 'Please enter a valid URL');
            return false;
        }
        this.clearError(input);
        return true;
    }
    validateNotes() {
        const textarea = this.elements.notesTextarea;
        if (!textarea)
            return true;
        const value = textarea.value.trim();
        if (value.length > 1000) {
            this.showError(textarea, 'Notes cannot exceed 1000 characters');
            return false;
        }
        this.clearError(textarea);
        return true;
    }
    showError(element, message) {
        const formGroup = element.closest('.form-group');
        if (!formGroup)
            return;
        // Find or create error span
        let errorSpan = formGroup.querySelector('.form-error');
        if (!errorSpan) {
            errorSpan = document.createElement('span');
            errorSpan.className = 'form-error';
            element.parentElement?.appendChild(errorSpan);
        }
        errorSpan.textContent = message;
        errorSpan.style.display = 'block';
        element.classList.add('input-validation-error');
        element.setAttribute('aria-invalid', 'true');
    }
    clearError(element) {
        const formGroup = element.closest('.form-group');
        if (!formGroup)
            return;
        const errorSpan = formGroup.querySelector('.form-error');
        if (errorSpan) {
            errorSpan.textContent = '';
            errorSpan.style.display = 'none';
        }
        element.classList.remove('input-validation-error');
        element.removeAttribute('aria-invalid');
    }
    validateForm() {
        let isValid = true;
        // Validate all required fields
        if (!this.validateTitle())
            isValid = false;
        if (!this.validateGenre())
            isValid = false;
        if (!this.validateIsbn())
            isValid = false;
        if (!this.validatePublishedYear())
            isValid = false;
        if (!this.validateBookStatus())
            isValid = false;
        if (!this.validateCoverUrl())
            isValid = false;
        if (!this.validateNotes())
            isValid = false;
        return isValid;
    }
    handleCoverUrlInput() {
        // Clear existing timer
        if (this.debounceTimer) {
            window.clearTimeout(this.debounceTimer);
        }
        // Set new timer to update preview after 500ms of no typing
        this.debounceTimer = window.setTimeout(() => {
            this.updateCoverPreview();
        }, 500);
    }
    checkInitialCoverUrl() {
        if (this.elements.coverUrlInput?.value) {
            this.updateCoverPreview();
        }
    }
    updateCoverPreview() {
        const coverUrl = this.elements.coverUrlInput?.value.trim();
        if (!coverUrl || !this.elements.coverPreview) {
            this.showPlaceholder();
            return;
        }
        // Validate URL format
        if (!this.isValidUrl(coverUrl)) {
            this.showPlaceholder();
            return;
        }
        // Create image element
        const img = document.createElement('img');
        img.onload = () => {
            this.showImage(coverUrl);
        };
        img.onerror = () => {
            this.showPlaceholder();
        };
        img.src = coverUrl;
    }
    showImage(url) {
        if (!this.elements.coverPreview)
            return;
        this.elements.coverPreview.className = 'cover-preview__image';
        this.elements.coverPreview.innerHTML = `<img src="${url}" alt="Book cover preview" />`;
    }
    showPlaceholder() {
        if (!this.elements.coverPreview)
            return;
        this.elements.coverPreview.className = 'cover-preview__placeholder';
        this.elements.coverPreview.id = 'coverPreview';
        this.elements.coverPreview.innerHTML = `
            <i class="bi bi-image"></i>
            <p>Cover Preview</p>
        `;
    }
    isValidUrl(url) {
        try {
            const urlObject = new URL(url);
            return urlObject.protocol === 'http:' || urlObject.protocol === 'https:';
        }
        catch {
            return false;
        }
    }
    handleFormSubmit(e) {
        // Run client-side validation
        if (!this.validateForm()) {
            e.preventDefault();
            // Scroll to first error
            const firstError = this.elements.form?.querySelector('.input-validation-error');
            if (firstError) {
                firstError.focus();
                firstError.scrollIntoView({ behavior: 'smooth', block: 'center' });
            }
            // Show error message
            this.showFormError('Please correct the errors before submitting.');
            return;
        }
        // Clear form-level errors if validation passes
        this.clearFormError();
    }
    showFormError(message) {
        // Find or create validation summary
        let validationSummary = this.elements.form?.querySelector('.validation-summary');
        if (!validationSummary) {
            validationSummary = document.createElement('div');
            validationSummary.className = 'validation-summary';
            this.elements.form?.insertBefore(validationSummary, this.elements.form.firstChild);
        }
        validationSummary.innerHTML = `
            <ul>
                <li>${message}</li>
            </ul>
        `;
        validationSummary.style.display = 'block';
        validationSummary.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
    clearFormError() {
        const validationSummary = this.elements.form?.querySelector('.validation-summary');
        if (validationSummary) {
            validationSummary.style.display = 'none';
            validationSummary.innerHTML = '';
        }
    }
}
// Initialize AddBook when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => new AddBook());
}
else {
    new AddBook();
}
export { AddBook };
//# sourceMappingURL=addbook.js.map