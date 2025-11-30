# Validation CSS Styling Guide

## Required CSS Classes

The validation system uses specific CSS classes to provide visual feedback. Ensure these styles are present in your CSS file.

## Core Validation Classes

### Error Input Styling
```css
/* Applied to inputs/textareas/selects that have validation errors */
.input-validation-error {
    border-color: #dc3545 !important;
    border-width: 2px;
    background-color: #fff5f5;
}

.input-validation-error:focus {
    outline-color: #dc3545;
    box-shadow: 0 0 0 0.2rem rgba(220, 53, 69, 0.25);
}
```

### Error Message Styling
```css
/* Applied to error message spans */
.form-error {
    display: none;
    color: #dc3545;
    font-size: 0.875rem;
    margin-top: 0.25rem;
    font-weight: 500;
}

.form-error:not(:empty) {
    display: block;
}
```

### Validation Summary Styling
```css
/* Applied to form-level error summary */
.validation-summary {
    display: none;
    background-color: #f8d7da;
    border: 1px solid #f5c2c7;
    border-radius: 0.375rem;
    padding: 1rem;
    margin-bottom: 1.5rem;
    color: #842029;
}

.validation-summary ul {
    margin: 0;
    padding-left: 1.5rem;
}

.validation-summary li {
    margin-bottom: 0.25rem;
}

.validation-summary li:last-child {
    margin-bottom: 0;
}
```

## Optional Enhancement Classes

### Success State (for future enhancement)
```css
.input-validation-success {
    border-color: #198754;
    background-color: #f1f9f5;
}

.input-validation-success:focus {
    outline-color: #198754;
    box-shadow: 0 0 0 0.2rem rgba(25, 135, 84, 0.25);
}
```

### Field Icon Indicators (for future enhancement)
```css
.form-group {
    position: relative;
}

.form-group .validation-icon {
    position: absolute;
    right: 12px;
    top: 50%;
    transform: translateY(-50%);
    pointer-events: none;
}

.form-group .validation-icon.error {
    color: #dc3545;
}

.form-group .validation-icon.success {
    color: #198754;
}
```

### Animated Transitions
```css
.form-input,
.form-select,
.form-textarea {
    transition: border-color 0.15s ease-in-out,
                background-color 0.15s ease-in-out,
                box-shadow 0.15s ease-in-out;
}

.form-error {
    transition: opacity 0.15s ease-in-out;
    opacity: 0;
}

.form-error:not(:empty) {
    opacity: 1;
}
```

## Form Group Structure

The validation expects this HTML structure:

```html
<div class="form-group">
    <label class="form-label">Field Name</label>
    <input type="text" class="form-input" />
    <span class="form-error"></span>
</div>
```

## Accessibility Considerations

### ARIA Attributes
The JavaScript automatically adds:
```html
<!-- On invalid field -->
<input aria-invalid="true" class="input-validation-error" />

<!-- When valid -->
<input aria-invalid="false" />
```

### Screen Reader Announcements
```css
/* Ensure error messages are announced */
.form-error[role="alert"] {
    /* Automatically announced by screen readers */
}
```

## Dark Mode Support (Optional)

```css
@media (prefers-color-scheme: dark) {
    .input-validation-error {
        border-color: #e74c5c;
        background-color: #2d1618;
    }

    .form-error {
        color: #f1aeb5;
    }

    .validation-summary {
        background-color: #2d1618;
        border-color: #5c1f28;
        color: #f1aeb5;
    }
}
```

## Mobile Responsive Adjustments

```css
@media (max-width: 768px) {
    .form-error {
        font-size: 0.75rem;
    }

    .validation-summary {
        padding: 0.75rem;
        font-size: 0.875rem;
    }
}
```

## Integration with Bootstrap (if used)

If your project uses Bootstrap, these classes align with Bootstrap's validation:

```css
/* Bootstrap compatible validation */
.was-validated .form-control:invalid,
.form-control.input-validation-error {
    border-color: #dc3545;
    padding-right: calc(1.5em + 0.75rem);
    background-image: url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 12 12' width='12' height='12' fill='none' stroke='%23dc3545'%3e%3ccircle cx='6' cy='6' r='4.5'/%3e%3cpath stroke-linejoin='round' d='M5.8 3.6h.4L6 6.5z'/%3e%3ccircle cx='6' cy='8.2' r='.6' fill='%23dc3545' stroke='none'/%3e%3c/svg%3e");
    background-repeat: no-repeat;
    background-position: right calc(0.375em + 0.1875rem) center;
    background-size: calc(0.75em + 0.375rem) calc(0.75em + 0.375rem);
}

.was-validated .form-control:valid,
.form-control.input-validation-success {
    border-color: #198754;
    padding-right: calc(1.5em + 0.75rem);
    background-image: url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 8 8'%3e%3cpath fill='%23198754' d='M2.3 6.73L.6 4.53c-.4-1.04.46-1.4 1.1-.8l1.1 1.4 3.4-3.8c.6-.63 1.6-.27 1.2.7l-4 4.6c-.43.5-.8.4-1.1.1z'/%3e%3c/svg%3e");
    background-repeat: no-repeat;
    background-position: right calc(0.375em + 0.1875rem) center;
    background-size: calc(0.75em + 0.375rem) calc(0.75em + 0.375rem);
}
```

## Testing CSS

To test if your CSS is working:

1. Add `.input-validation-error` class to an input manually
2. Add text to a `.form-error` span
3. Verify colors and styling appear correctly

```html
<!-- Test markup -->
<div class="form-group">
    <label class="form-label">Test Field</label>
    <input type="text" class="form-input input-validation-error" value="Invalid" />
    <span class="form-error">This is an error message</span>
</div>
```

## CSS File Location

Add these styles to:
- `HomeLibrary.Web.Mvc/wwwroot/css/site.css`

Or create a dedicated validation stylesheet:
- `HomeLibrary.Web.Mvc/wwwroot/css/validation.css`

And reference it in `_Layout.cshtml`:
```html
<link rel="stylesheet" href="~/css/validation.css" asp-append-version="true"/>
```

## Color Palette

| State | Color | Hex Code |
|-------|-------|----------|
| Error | Red | `#dc3545` |
| Success | Green | `#198754` |
| Warning | Yellow | `#ffc107` |
| Info | Blue | `#0dcaf0` |

## Animation Timing

```css
/* Recommended timing functions */
:root {
    --validation-transition-speed: 0.15s;
    --validation-easing: ease-in-out;
}

.form-input {
    transition: all var(--validation-transition-speed) var(--validation-easing);
}
```

## Print Styles

```css
@media print {
    .input-validation-error {
        border: 2px solid #000 !important;
    }

    .form-error {
        color: #000 !important;
        font-weight: bold;
    }

    .validation-summary {
        border: 2px solid #000 !important;
        background-color: #fff !important;
        color: #000 !important;
    }
}
```

## Notes

- The CSS classes are already referenced in the TypeScript/JavaScript code
- Ensure these styles match your application's design system
- Adjust colors to match your brand guidelines
- Test with different screen sizes and devices
- Verify color contrast meets WCAG 2.1 AA standards (minimum 4.5:1 ratio)
