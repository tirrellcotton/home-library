/**
 * Wishlist Module
 * Handles search and interactions for the My Wishlist page
 */
import { BookModal } from './bookmodal.js';
class Wishlist {
    constructor() {
        this.debounceTimer = null;
        this.elements = {
            searchInput: document.querySelector('.search-bar__input'),
            filterForm: document.querySelector('.filter-form'),
            bookCards: document.querySelectorAll('.book-card')
        };
        this.bookModal = new BookModal();
        this.init();
    }
    init() {
        this.attachEventListeners();
    }
    attachEventListeners() {
        // Auto-submit on search input with debounce
        this.elements.searchInput?.addEventListener('input', () => this.handleSearchInput());
        // Prevent default form submission
        this.elements.filterForm?.addEventListener('submit', (e) => this.handleFormSubmit(e));
        // Book card clicks to open modal
        this.elements.bookCards.forEach(card => {
            card.addEventListener('click', (e) => this.handleBookCardClick(e, card));
        });
    }
    handleSearchInput() {
        // Clear existing timer
        if (this.debounceTimer) {
            window.clearTimeout(this.debounceTimer);
        }
        // Set new timer to submit after 500ms of no typing
        this.debounceTimer = window.setTimeout(() => {
            this.submitForm();
        }, 500);
    }
    handleFormSubmit(e) {
        e.preventDefault();
        this.submitForm();
    }
    submitForm() {
        if (this.elements.filterForm) {
            this.elements.filterForm.submit();
        }
    }
    handleBookCardClick(e, card) {
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
}
else {
    new Wishlist();
}
export { Wishlist };
//# sourceMappingURL=wishlist.js.map