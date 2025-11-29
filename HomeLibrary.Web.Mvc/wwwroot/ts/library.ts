/**
 * Library Module
 * Handles search, filtering, and interactions for the My Library page
 */

interface LibraryElements {
    searchInput: HTMLInputElement | null;
    genreSelect: HTMLSelectElement | null;
    authorSelect: HTMLSelectElement | null;
    filterForm: HTMLFormElement | null;
}

class Library {
    private elements: LibraryElements;
    private debounceTimer: number | null = null;

    constructor() {
        this.elements = {
            searchInput: document.querySelector('.search-bar__input'),
            genreSelect: document.querySelector('select[name="genreId"]'),
            authorSelect: document.querySelector('select[name="authorId"]'),
            filterForm: document.querySelector('.filter-form')
        };

        this.init();
    }

    private init(): void {
        this.attachEventListeners();
    }

    private attachEventListeners(): void {
        // Auto-submit on search input with debounce
        this.elements.searchInput?.addEventListener('input', () => this.handleSearchInput());

        // Auto-submit on filter change
        this.elements.genreSelect?.addEventListener('change', () => this.submitForm());
        this.elements.authorSelect?.addEventListener('change', () => this.submitForm());

        // Prevent default form submission
        this.elements.filterForm?.addEventListener('submit', (e) => this.handleFormSubmit(e));
    }

    private handleSearchInput(): void {
        // Clear existing timer
        if (this.debounceTimer) {
            window.clearTimeout(this.debounceTimer);
        }

        // Set new timer to submit after 500ms of no typing
        this.debounceTimer = window.setTimeout(() => {
            this.submitForm();
        }, 500);
    }

    private handleFormSubmit(e: Event): void {
        e.preventDefault();
        this.submitForm();
    }

    private submitForm(): void {
        if (this.elements.filterForm) {
            this.elements.filterForm.submit();
        }
    }

    // Future enhancement: Voice search functionality
    private handleVoiceSearch(): void {
        // Implementation for voice search can be added here
        console.log('Voice search not yet implemented');
    }
}

// Initialize library when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => new Library());
} else {
    new Library();
}

export { Library };
