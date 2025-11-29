/**
 * Dashboard Module
 * Handles mobile menu interactions and UI functionality for the Home Library dashboard
 */

interface DashboardElements {
    menuBtn: HTMLButtonElement | null;
    moreBtn: HTMLButtonElement | null;
    closeMenuBtn: HTMLButtonElement | null;
    sideMenu: HTMLElement | null;
    menuOverlay: HTMLElement | null;
    dropdownMenu: HTMLElement | null;
}

class Dashboard {
    private elements: DashboardElements;

    constructor() {
        this.elements = {
            menuBtn: document.getElementById('menuBtn') as HTMLButtonElement,
            moreBtn: document.getElementById('moreBtn') as HTMLButtonElement,
            closeMenuBtn: document.getElementById('closeMenuBtn') as HTMLButtonElement,
            sideMenu: document.getElementById('sideMenu') as HTMLElement,
            menuOverlay: document.getElementById('menuOverlay') as HTMLElement,
            dropdownMenu: document.getElementById('dropdownMenu') as HTMLElement
        };

        this.init();
    }

    private init(): void {
        this.attachEventListeners();
    }

    private attachEventListeners(): void {
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

    private openSideMenu(): void {
        this.elements.sideMenu?.classList.add('show');
        this.elements.menuOverlay?.classList.add('show');
        document.body.style.overflow = 'hidden';
    }

    private closeSideMenu(): void {
        this.elements.sideMenu?.classList.remove('show');
        this.elements.menuOverlay?.classList.remove('show');
        document.body.style.overflow = '';
    }

    private toggleDropdown(e: Event): void {
        e.stopPropagation();
        this.elements.dropdownMenu?.classList.toggle('show');
    }

    private closeDropdown(): void {
        this.elements.dropdownMenu?.classList.remove('show');
    }

    private handleOutsideClick(e: Event): void {
        const target = e.target as HTMLElement;
        
        if (!this.elements.dropdownMenu?.contains(target) && 
            !this.elements.moreBtn?.contains(target)) {
            this.closeDropdown();
        }
    }

    private handleEscapeKey(e: KeyboardEvent): void {
        if (e.key === 'Escape') {
            this.closeSideMenu();
            this.closeDropdown();
        }
    }
}

// Initialize dashboard when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => new Dashboard());
} else {
    new Dashboard();
}

export { Dashboard };
