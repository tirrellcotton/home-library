/**
 * Wishlist Module
 * Handles search and interactions for the My Wishlist page
 */

import { BookModal } from './bookmodal.js';

interface WishlistElements {
    searchInput: HTMLInputElement | null;
    filterForm: HTMLFormElement | null;
    bookCards: NodeListOf<HTMLElement>;
}

class Wishlist {
    private elements: WishlistElements;
    private debounceTimer: number | null = null;
    private bookModal: BookModal;

    constructor() {
        this.elements = {
            searchInput: document.querySelector('.search-bar__input'),
            filterForm: document.querySelector('.filter-form'),
            bookCards: document.querySelectorAll('.book-card')
        };

        this.bookModal = new BookModal();
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

        // Book card clicks to open modal
        this.elements.bookCards.forEach(card => {
            card.addEventListener('click', (e) => this.handleBookCardClick(e, card));
        });
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

    private handleBookCardClick(e: Event, card: HTMLElement): void {
        const bookId = card.getAttribute('data-book-id');
        
        if (bookId) {
            const bookIdNum = parseInt(bookId, 10);
            this.bookModal.open(bookIdNum);
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
