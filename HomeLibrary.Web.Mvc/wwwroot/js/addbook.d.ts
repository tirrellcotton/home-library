/**
 * AddBook Module
 * Handles form interactions, cover image preview, and client-side validation for the Add Book page
 */
declare class AddBook {
    private elements;
    private debounceTimer;
    private validationTimers;
    constructor();
    private init;
    private attachEventListeners;
    private debouncedValidate;
    private validateTitle;
    private validateGenre;
    private validateIsbn;
    private validatePublishedYear;
    private validateBookStatus;
    private validateCoverUrl;
    private validateNotes;
    private showError;
    private clearError;
    private validateForm;
    private handleCoverUrlInput;
    private checkInitialCoverUrl;
    private updateCoverPreview;
    private showImage;
    private showPlaceholder;
    private isValidUrl;
    private handleFormSubmit;
    private showFormError;
    private clearFormError;
}
export { AddBook };
