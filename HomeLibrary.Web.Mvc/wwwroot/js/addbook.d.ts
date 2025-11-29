/**
 * AddBook Module
 * Handles form interactions and cover image preview for the Add Book page
 */
declare class AddBook {
    private elements;
    private debounceTimer;
    constructor();
    private init;
    private attachEventListeners;
    private handleCoverUrlInput;
    private checkInitialCoverUrl;
    private updateCoverPreview;
    private showImage;
    private showPlaceholder;
    private isValidUrl;
    private handleFormSubmit;
}
export { AddBook };
