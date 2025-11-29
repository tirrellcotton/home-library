/**
 * Wishlist Module
 * Handles search and interactions for the My Wishlist page
 */
declare class Wishlist {
    private elements;
    private debounceTimer;
    private bookModal;
    constructor();
    private init;
    private attachEventListeners;
    private handleSearchInput;
    private handleFormSubmit;
    private submitForm;
    private handleBookCardClick;
}
export { Wishlist };
