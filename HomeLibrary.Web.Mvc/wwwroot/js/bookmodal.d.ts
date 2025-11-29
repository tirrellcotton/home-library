/**
 * BookModal Module
 * Handles book details modal display and interactions
 */
declare class BookModal {
    private elements;
    constructor();
    private init;
    private attachEventListeners;
    open(bookId: number): Promise<void>;
    private populateModal;
    private setOptionalField;
    private showModal;
    close(): void;
    private handleEscapeKey;
}
export { BookModal };
