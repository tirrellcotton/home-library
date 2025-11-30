# Client-Side Validation Documentation

## Overview

Client-side validation has been implemented for the Add Book and Edit Book forms to provide immediate feedback to users and improve the overall user experience.

## Features

### 1. **Real-Time Validation**
- Fields are validated as users type (with 500ms debounce to avoid excessive validation)
- Validation also triggers on blur (when user leaves a field)
- Immediate visual feedback with error messages

### 2. **Validation Rules**

#### **Title** (Required)
- Cannot be empty
- Maximum 200 characters
- Error messages:
  - "Title is required"
  - "Title cannot exceed 200 characters"

#### **Genre** (Required)
- Must select a genre from dropdown
- Error message: "Genre is required"

#### **ISBN** (Optional)
- If provided, maximum 50 characters
- Must be 10 or 13 digits (after removing spaces and hyphens)
- Must contain only numbers
- Error messages:
  - "ISBN cannot exceed 50 characters"
  - "ISBN must be 10 or 13 digits"
  - "ISBN must contain only numbers"

#### **Published Year** (Optional)
- If provided, must be between 1000-9999
- Cannot be more than 1 year in the future
- Error messages:
  - "Please enter a valid year"
  - "Please enter a valid year (1000-9999)"
  - "Year cannot be greater than [current year + 1]"

#### **Book Status** (Required)
- Must select a status from dropdown
- Error message: "Status is required"

#### **Cover Image URL** (Optional)
- If provided, must be a valid HTTP/HTTPS URL
- Error message: "Please enter a valid URL"

#### **Notes** (Optional)
- Maximum 1000 characters
- Error message: "Notes cannot exceed 1000 characters"

### 3. **Form Submission Validation**
- All fields are validated before form submission
- If validation fails:
  - Form submission is prevented
  - First invalid field is focused and scrolled into view
  - Summary error message is displayed at top of form
  - Individual field errors are shown below each field

### 4. **Visual Feedback**
- Invalid fields are highlighted with red border (`.input-validation-error` class)
- Error messages appear below each field in red text
- ARIA attributes (`aria-invalid`) are set for accessibility
- Error messages are cleared automatically when field becomes valid

### 5. **Debouncing**
- Input validation is debounced (500ms delay) to avoid excessive validation while typing
- Improves performance and reduces visual noise
- Immediate validation on blur for quick feedback

## Technical Implementation

### TypeScript Structure

```typescript
class AddBook {
    // Elements
    private elements: AddBookElements;
    
    // Timers for debouncing
    private debounceTimer: number | null = null;
    private validationTimers: Map<string, number> = new Map();
    
    // Methods
    private validateTitle(): boolean
    private validateGenre(): boolean
    private validateIsbn(): boolean
    private validatePublishedYear(): boolean
    private validateBookStatus(): boolean
    private validateCoverUrl(): boolean
    private validateNotes(): boolean
    private validateForm(): boolean
    private showError(element, message): void
    private clearError(element): void
}
```

### Event Listeners

- **blur**: Immediate validation when user leaves field
- **input**: Debounced validation while typing (500ms delay)
- **change**: Immediate validation for dropdowns
- **submit**: Full form validation before submission

### Integration with Server-Side Validation

- Client-side validation complements server-side validation
- ASP.NET Core validation attributes on view models still apply
- jQuery Unobtrusive Validation is still included for fallback
- Both layers work together for robust validation

## Browser Support

- Modern browsers with ES2020 support
- Uses native browser APIs:
  - `URL()` for URL validation
  - `querySelector()` for DOM manipulation
  - `setTimeout()` for debouncing
  - Native form validation APIs

## Accessibility

- Error messages are associated with form fields
- `aria-invalid` attribute set on invalid fields
- Keyboard navigation fully supported
- Screen reader friendly error announcements
- Focus management directs user to first error

## Testing the Validation

### To Test Required Fields:
1. Leave Title or Genre empty
2. Try to submit the form
3. Observe error messages

### To Test ISBN Validation:
1. Enter invalid ISBN (e.g., "123")
2. Leave the field
3. Observe error message
4. Enter valid 10 or 13 digit ISBN

### To Test Year Validation:
1. Enter year < 1000 or > 9999
2. Enter future year (e.g., 2030)
3. Observe appropriate error messages

### To Test URL Validation:
1. Enter invalid URL in Cover Image URL
2. Observe error message
3. Enter valid HTTP/HTTPS URL

### To Test Character Limits:
1. Enter more than 200 characters in Title
2. Enter more than 1000 characters in Notes
3. Observe character limit error messages

## Customization

To add validation to additional fields:

1. Add field reference to `AddBookElements` interface
2. Create validation method (e.g., `validateNewField()`)
3. Add event listeners in `attachEventListeners()`
4. Include validation in `validateForm()` method

## Performance Considerations

- Debouncing prevents excessive validation calls
- Validation only runs on user interaction
- Minimal DOM manipulation
- Efficient error display/clearing

## Future Enhancements

Potential improvements:
- Custom ISBN checksum validation
- Integration with ISBN lookup API
- Cover image dimension validation
- Duplicate book title checking
- Rich text editor for notes with formatting
- Character counter for text fields
