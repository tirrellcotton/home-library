/**
 * Wishlist Module
 * Handles search and interactions for the My Wishlist page
 */
class Wishlist {
    constructor() {
        this.debounceTimer = null;
        this.elements = {
            searchInput: document.querySelector('.search-bar__input'),
            filterForm: document.querySelector('.filter-form')
        };
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