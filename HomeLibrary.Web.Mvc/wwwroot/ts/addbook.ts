/**
 * AddBook Module
 * Handles form interactions and cover image preview for the Add Book page
 */

interface AddBookElements {
    coverUrlInput: HTMLTextAreaElement | null;
    coverPreview: HTMLElement | null;
    form: HTMLFormElement | null;
}

class AddBook {
    private elements: AddBookElements;
    private debounceTimer: number | null = null;

    constructor() {
        this.elements = {
            coverUrlInput: document.querySelector('textarea[name="CoverImageUrl"]'),
            coverPreview: document.getElementById('coverPreview'),
            form: document.querySelector('.add-book-form')
        };

        this.init();
    }

    private init(): void {
        this.attachEventListeners();
        this.checkInitialCoverUrl();
    }

    private attachEventListeners(): void {
        // Cover URL input with debounce
        this.elements.coverUrlInput?.addEventListener('input', () => this.handleCoverUrlInput());

        // Form submission
        this.elements.form?.addEventListener('submit', (e) => this.handleFormSubmit(e));
    }

    private handleCoverUrlInput(): void {
        // Clear existing timer
        if (this.debounceTimer) {
            window.clearTimeout(this.debounceTimer);
        }

        // Set new timer to update preview after 500ms of no typing
        this.debounceTimer = window.setTimeout(() => {
            this.updateCoverPreview();
        }, 500);
    }

    private checkInitialCoverUrl(): void {
        if (this.elements.coverUrlInput?.value) {
            this.updateCoverPreview();
        }
    }

    private updateCoverPreview(): void {
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

    private showImage(url: string): void {
        if (!this.elements.coverPreview) return;

        this.elements.coverPreview.className = 'cover-preview__image';
        this.elements.coverPreview.innerHTML = `<img src="${url}" alt="Book cover preview" />`;
    }

    private showPlaceholder(): void {
        if (!this.elements.coverPreview) return;

        this.elements.coverPreview.className = 'cover-preview__placeholder';
        this.elements.coverPreview.id = 'coverPreview';
        this.elements.coverPreview.innerHTML = `
            <i class="bi bi-image"></i>
            <p>Cover Preview</p>
        `;
    }

    private isValidUrl(url: string): boolean {
        try {
            new URL(url);
            return true;
        } catch {
            return false;
        }
    }

    private handleFormSubmit(e: Event): void {
        // Form validation will be handled by ASP.NET Core MVC
        // This is a placeholder for any client-side validation you might want to add
        console.log('Form submitted');
    }
}

// Initialize AddBook when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => new AddBook());
} else {
    new AddBook();
}

export { AddBook };
