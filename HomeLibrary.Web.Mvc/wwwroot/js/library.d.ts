/**
 * Library Module
 * Handles search, filtering, and interactions for the My Library page
 */
declare class Library {
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
export { Library };
