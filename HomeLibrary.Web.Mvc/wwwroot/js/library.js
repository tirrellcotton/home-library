/**
 * Library Module
 * Handles search, filtering, and interactions for the My Library page
 */
import { BookModal } from './bookmodal.js';
class Library {
    constructor() {
        this.debounceTimer = null;
        this.elements = {
            searchInput: document.querySelector('.search-bar__input'),
            genreSelect: document.querySelector('select[name="genreId"]'),
            authorSelect: document.querySelector('select[name="authorId"]'),
            filterForm: document.querySelector('.filter-form'),
            bookCards: document.querySelectorAll('.book-card')
        };
        console.log('Library initialized');
        console.log('Found book cards:', this.elements.bookCards.length);
        this.bookModal = new BookModal();
        this.init();
    }
    init() {
        this.attachEventListeners();
    }
    attachEventListeners() {
        // Auto-submit on search input with debounce
        this.elements.searchInput?.addEventListener('input', () => this.handleSearchInput());
        // Auto-submit on filter change
        this.elements.genreSelect?.addEventListener('change', () => this.submitForm());
        this.elements.authorSelect?.addEventListener('change', () => this.submitForm());
        // Prevent default form submission
        this.elements.filterForm?.addEventListener('submit', (e) => this.handleFormSubmit(e));
        // Book card clicks to open modal
        this.elements.bookCards.forEach((card, index) => {
            console.log(`Attaching click handler to card ${index}`, card);
            card.addEventListener('click', (e) => this.handleBookCardClick(e, card));
        });
        console.log('Event listeners attached');
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
        console.log('Book card clicked!', card);
        const bookId = card.getAttribute('data-book-id');
        console.log('Book ID:', bookId);
        if (bookId) {
            const bookIdNum = parseInt(bookId, 10);
            console.log('Opening modal for book ID:', bookIdNum);
            this.bookModal.open(bookIdNum);
        }
        else {
            console.error('No book ID found on card');
        }
    }
}
// Initialize library when DOM is ready
console.log('Library module loaded');
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => {
        console.log('DOM Content Loaded - initializing Library');
        new Library();
    });
}
else {
    console.log('DOM already loaded - initializing Library immediately');
    new Library();
}
export { Library };
//# sourceMappingURL=library.js.map