/**
 * AddBook Module
 * Handles form interactions and cover image preview for the Add Book page
 */
class AddBook {
    constructor() {
        this.debounceTimer = null;
        this.elements = {
            coverUrlInput: document.querySelector('textarea[name="CoverImageUrl"]'),
            coverPreview: document.getElementById('coverPreview'),
            form: document.querySelector('.add-book-form')
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
            new URL(url);
            return true;
        }
        catch {
            return false;
        }
    }
    handleFormSubmit(e) {
        // Form validation will be handled by ASP.NET Core MVC
        // This is a placeholder for any client-side validation you might want to add
        console.log('Form submitted');
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