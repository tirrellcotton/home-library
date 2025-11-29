/**
 * Wishlist Module
 * Handles search and interactions for the My Wishlist page
 */

interface WishlistElements {
    searchInput: HTMLInputElement | null;
    filterForm: HTMLFormElement | null;
}

class Wishlist {
    private elements: WishlistElements;
    private debounceTimer: number | null = null;

    constructor() {
        this.elements = {
            searchInput: document.querySelector('.search-bar__input'),
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
}

// Initialize wishlist when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => new Wishlist());
} else {
    new Wishlist();
}

export { Wishlist };
