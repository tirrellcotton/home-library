/**
 * Dashboard Module
 * Handles mobile menu interactions and UI functionality for the Home Library dashboard
 */
class Dashboard {
    constructor() {
        this.elements = {
            menuBtn: document.getElementById('menuBtn'),
            moreBtn: document.getElementById('moreBtn'),
            closeMenuBtn: document.getElementById('closeMenuBtn'),
            sideMenu: document.getElementById('sideMenu'),
            menuOverlay: document.getElementById('menuOverlay'),
            dropdownMenu: document.getElementById('dropdownMenu')
        };
        this.init();
    }
    init() {
        this.attachEventListeners();
    }
    attachEventListeners() {
        // Hamburger menu button
        this.elements.menuBtn?.addEventListener('click', () => this.openSideMenu());
        // Close menu button
        this.elements.closeMenuBtn?.addEventListener('click', () => this.closeSideMenu());
        // Menu overlay click
        this.elements.menuOverlay?.addEventListener('click', () => this.closeSideMenu());
        // More options button (three dots)
        this.elements.moreBtn?.addEventListener('click', (e) => this.toggleDropdown(e));
        // Close dropdown when clicking outside
        document.addEventListener('click', (e) => this.handleOutsideClick(e));
        // Handle escape key
        document.addEventListener('keydown', (e) => this.handleEscapeKey(e));
    }
    openSideMenu() {
        this.elements.sideMenu?.classList.add('show');
        this.elements.menuOverlay?.classList.add('show');
        document.body.style.overflow = 'hidden';
    }
    closeSideMenu() {
        this.elements.sideMenu?.classList.remove('show');
        this.elements.menuOverlay?.classList.remove('show');
        document.body.style.overflow = '';
    }
    toggleDropdown(e) {
        e.stopPropagation();
        this.elements.dropdownMenu?.classList.toggle('show');
    }
    closeDropdown() {
        this.elements.dropdownMenu?.classList.remove('show');
    }
    handleOutsideClick(e) {
        const target = e.target;
        if (!this.elements.dropdownMenu?.contains(target) &&
            !this.elements.moreBtn?.contains(target)) {
            this.closeDropdown();
        }
    }
    handleEscapeKey(e) {
        if (e.key === 'Escape') {
            this.closeSideMenu();
            this.closeDropdown();
        }
    }
}
// Initialize dashboard when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => new Dashboard());
}
else {
    new Dashboard();
}
export { Dashboard };
//# sourceMappingURL=dashboard.js.map