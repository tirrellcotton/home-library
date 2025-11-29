/**
 * BookModal Module
 * Handles book details modal display and interactions
 */
class BookModal {
    constructor() {
        console.log('BookModal constructor called');
        this.elements = {
            modal: document.getElementById('bookDetailsModal'),
            overlay: document.getElementById('modalOverlay'),
            closeBtn: document.getElementById('modalClose'),
            cover: document.getElementById('modalCover'),
            title: document.getElementById('modalTitle'),
            author: document.getElementById('modalAuthor'),
            authorContainer: document.getElementById('modalAuthorContainer'),
            genre: document.getElementById('modalGenre'),
            genreContainer: document.getElementById('modalGenreContainer'),
            publisher: document.getElementById('modalPublisher'),
            publisherContainer: document.getElementById('modalPublisherContainer'),
            year: document.getElementById('modalYear'),
            yearContainer: document.getElementById('modalYearContainer'),
            isbn: document.getElementById('modalIsbn'),
            isbnContainer: document.getElementById('modalIsbnContainer'),
            notes: document.getElementById('modalNotes'),
            notesContainer: document.getElementById('modalNotesContainer'),
            editBtn: document.getElementById('modalEditBtn')
        };
        console.log('Modal element found:', !!this.elements.modal);
        console.log('Modal elements:', this.elements);
        this.init();
    }
    init() {
        this.attachEventListeners();
        console.log('BookModal initialized');
    }
    attachEventListeners() {
        // Close button
        this.elements.closeBtn?.addEventListener('click', () => this.close());
        // Overlay click
        this.elements.overlay?.addEventListener('click', () => this.close());
        // Escape key
        document.addEventListener('keydown', (e) => this.handleEscapeKey(e));
    }
    async open(bookId) {
        console.log('BookModal.open called with bookId:', bookId);
        try {
            console.log('Fetching book details from API...');
            // Fetch book details from API
            const response = await fetch(`/api/books/${bookId}`);
            console.log('API response status:', response.status);
            if (!response.ok) {
                throw new Error('Failed to fetch book details');
            }
            const book = await response.json();
            console.log('Book data received:', book);
            this.populateModal(book);
            this.showModal();
        }
        catch (error) {
            console.error('Error loading book details:', error);
            alert('Unable to load book details. Please try again.');
        }
    }
    populateModal(book) {
        // Set title
        if (this.elements.title) {
            this.elements.title.textContent = book.title;
        }
        // Set cover image
        if (this.elements.cover) {
            if (book.coverImageUrl) {
                this.elements.cover.className = 'modal-cover__image';
                this.elements.cover.innerHTML = `<img src="${book.coverImageUrl}" alt="${book.title} cover" />`;
            }
            else {
                this.elements.cover.className = 'modal-cover__placeholder';
                this.elements.cover.innerHTML = '<i class="bi bi-book"></i>';
            }
        }
        // Set author
        this.setOptionalField(this.elements.authorContainer, this.elements.author, book.author);
        // Set genre
        this.setOptionalField(this.elements.genreContainer, this.elements.genre, book.genre);
        // Set publisher
        this.setOptionalField(this.elements.publisherContainer, this.elements.publisher, book.publisher);
        // Set published year
        this.setOptionalField(this.elements.yearContainer, this.elements.year, book.publishedYear?.toString());
        // Set ISBN
        this.setOptionalField(this.elements.isbnContainer, this.elements.isbn, book.isbn);
        // Set notes
        if (book.notes && book.notes.trim()) {
            if (this.elements.notesContainer) {
                this.elements.notesContainer.style.display = 'block';
            }
            if (this.elements.notes) {
                this.elements.notes.textContent = book.notes;
            }
        }
        else {
            if (this.elements.notesContainer) {
                this.elements.notesContainer.style.display = 'none';
            }
        }
        // Set edit button link
        if (this.elements.editBtn) {
            this.elements.editBtn.href = `/AddBook/Edit/${book.id}`;
        }
    }
    setOptionalField(container, element, value) {
        if (value && value.trim()) {
            if (container) {
                container.style.display = 'flex';
            }
            if (element) {
                element.textContent = value;
            }
        }
        else {
            if (container) {
                container.style.display = 'none';
            }
        }
    }
    showModal() {
        if (this.elements.modal) {
            this.elements.modal.classList.add('show');
            document.body.classList.add('modal-open');
        }
    }
    close() {
        if (this.elements.modal) {
            this.elements.modal.classList.remove('show');
            document.body.classList.remove('modal-open');
        }
    }
    handleEscapeKey(e) {
        if (e.key === 'Escape' && this.elements.modal?.classList.contains('show')) {
            this.close();
        }
    }
}
export { BookModal };
//# sourceMappingURL=bookmodal.js.map