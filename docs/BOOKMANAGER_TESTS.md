# BookManager Unit Tests Documentation

## Overview

Comprehensive unit test suite for the `BookManager` class with 34 test cases covering all public methods and edge cases.

## Test Summary

| Category | Tests | Description |
|----------|-------|-------------|
| **GetOwnedBookCountAsync** | 3 | Count of owned books |
| **GetWishlistBookCountAsync** | 3 | Count of wishlist books |
| **GetOwnedBooksAsync** | 10 | Retrieve owned books with filtering |
| **GetWishlistBooksAsync** | 6 | Retrieve wishlist books with search |
| **AddBookAsync** | 4 | Add new books to database |
| **GetBookByIdAsync** | 4 | Retrieve book by ID with relations |
| **UpdateBookAsync** | 4 | Update existing books |
| **Total** | **34** | All tests passing ? |

## Test Results

```
Test Run Successful.
Total tests: 34
     Passed: 34
     Failed: 0
  Skipped: 0
Total time: 2.2s
```

## Detailed Test Cases

### 1. GetOwnedBookCountAsync (3 tests)

#### ? GetOwnedBookCountAsync_ReturnsCorrectCount
**Purpose**: Verify correct count of owned books when mixed statuses exist  
**Setup**: 2 owned books, 1 wishlist book  
**Expected**: Count = 2  
**Result**: ? Passed

#### ? GetOwnedBookCountAsync_WithNoOwnedBooks_ReturnsZero
**Purpose**: Verify zero count when no owned books exist  
**Setup**: Only wishlist and read books  
**Expected**: Count = 0  
**Result**: ? Passed

#### ? GetOwnedBookCountAsync_WithEmptyDatabase_ReturnsZero
**Purpose**: Verify zero count with empty database  
**Setup**: No books in database  
**Expected**: Count = 0  
**Result**: ? Passed

---

### 2. GetWishlistBookCountAsync (3 tests)

#### ? GetWishlistBookCountAsync_ReturnsCorrectCount
**Purpose**: Verify correct count of wishlist books  
**Setup**: 2 wishlist books, 1 owned book  
**Expected**: Count = 2  
**Result**: ? Passed

#### ? GetWishlistBookCountAsync_WithNoWishlistBooks_ReturnsZero
**Purpose**: Verify zero count when no wishlist books exist  
**Setup**: Only owned and read books  
**Expected**: Count = 0  
**Result**: ? Passed

#### ? GetWishlistBookCountAsync_WithEmptyDatabase_ReturnsZero
**Purpose**: Verify zero count with empty database  
**Setup**: No books in database  
**Expected**: Count = 0  
**Result**: ? Passed

---

### 3. GetOwnedBooksAsync (10 tests)

#### ? GetOwnedBooksAsync_WithNoFilters_ReturnsAllOwnedBooks
**Purpose**: Retrieve all owned books without filters  
**Setup**: 2 owned books, 1 wishlist book  
**Expected**: Returns 2 owned books only  
**Result**: ? Passed

#### ? GetOwnedBooksAsync_WithSearchTerm_ReturnsMatchingBooks
**Purpose**: Filter books by title search term  
**Setup**: Books with "Great" in title  
**Search**: "Great"  
**Expected**: Returns 2 books containing "Great"  
**Result**: ? Passed

#### ? GetOwnedBooksAsync_WithGenreFilter_ReturnsMatchingBooks
**Purpose**: Filter books by genre ID  
**Setup**: Books with different genres  
**Filter**: GenreId = 1  
**Expected**: Returns books with GenreId = 1  
**Result**: ? Passed

#### ? GetOwnedBooksAsync_WithAuthorFilter_ReturnsMatchingBooks
**Purpose**: Filter books by author ID  
**Setup**: Books with different authors  
**Filter**: AuthorId = 1  
**Expected**: Returns books by Author One  
**Result**: ? Passed

#### ? GetOwnedBooksAsync_WithAllFilters_ReturnsMatchingBooks
**Purpose**: Apply all filters simultaneously  
**Setup**: Multiple books with various attributes  
**Filters**: SearchTerm + GenreId + AuthorId  
**Expected**: Returns 1 book matching all criteria  
**Result**: ? Passed

#### ? GetOwnedBooksAsync_WithZeroGenreId_IgnoresGenreFilter
**Purpose**: Verify GenreId=0 doesn't filter  
**Setup**: Books with different genres  
**Filter**: GenreId = 0  
**Expected**: Returns all books (filter ignored)  
**Result**: ? Passed

#### ? GetOwnedBooksAsync_WithZeroAuthorId_IgnoresAuthorFilter
**Purpose**: Verify AuthorId=0 doesn't filter  
**Setup**: Books with different authors  
**Filter**: AuthorId = 0  
**Expected**: Returns all books (filter ignored)  
**Result**: ? Passed

#### ? GetOwnedBooksAsync_OrdersByTitle
**Purpose**: Verify results are sorted alphabetically  
**Setup**: Books named "Zebra", "Apple", "Mango"  
**Expected**: Returns in order: Apple, Mango, Zebra  
**Result**: ? Passed

#### ? GetOwnedBooksAsync_WithNoMatches_ReturnsEmptyList
**Purpose**: Verify empty result when no matches  
**Setup**: Books without search term  
**Search**: "Nonexistent"  
**Expected**: Empty list  
**Result**: ? Passed

#### ? GetOwnedBooksAsync_WithNullAuthor_HandlesCorrectly
**Purpose**: Verify handling of books without authors  
**Setup**: 1 book without author, 1 with author  
**Expected**: Returns both, null for book without author  
**Result**: ? Passed

---

### 4. GetWishlistBooksAsync (6 tests)

#### ? GetWishlistBooksAsync_WithNoSearchTerm_ReturnsAllWishlistBooks
**Purpose**: Retrieve all wishlist books without search  
**Setup**: 2 wishlist books, 1 owned book  
**Expected**: Returns 2 wishlist books only  
**Result**: ? Passed

#### ? GetWishlistBooksAsync_WithSearchTerm_ReturnsMatchingBooks
**Purpose**: Filter wishlist by title search  
**Setup**: Books with "Great" in title  
**Search**: "Great"  
**Expected**: Returns 2 books containing "Great"  
**Result**: ? Passed

#### ? GetWishlistBooksAsync_OrdersByTitle
**Purpose**: Verify alphabetical sorting  
**Setup**: Books named "Zebra", "Apple", "Mango"  
**Expected**: Returns in order: Apple, Mango, Zebra  
**Result**: ? Passed

#### ? GetWishlistBooksAsync_WithEmptyDatabase_ReturnsEmptyList
**Purpose**: Handle empty database gracefully  
**Setup**: No books in database  
**Expected**: Empty list  
**Result**: ? Passed

#### ? GetWishlistBooksAsync_WithNoMatches_ReturnsEmptyList
**Purpose**: Verify empty result when no matches  
**Setup**: Books without search term  
**Search**: "Nonexistent"  
**Expected**: Empty list  
**Result**: ? Passed

#### ? GetWishlistBooksAsync_WithNullAuthor_HandlesCorrectly
**Purpose**: Verify handling of books without authors  
**Setup**: 1 book without author, 1 with author  
**Expected**: Returns both, null for book without author  
**Result**: ? Passed

---

### 5. AddBookAsync (4 tests)

#### ? AddBookAsync_AddsBookToDatabase
**Purpose**: Verify book is persisted to database  
**Setup**: New book with all required fields  
**Expected**: Book saved with generated ID  
**Result**: ? Passed

#### ? AddBookAsync_ReturnsGeneratedId
**Purpose**: Verify method returns the generated ID  
**Setup**: New book  
**Expected**: Returns ID > 0  
**Result**: ? Passed

#### ? AddBookAsync_WithMinimalData_Succeeds
**Purpose**: Verify minimal required fields work  
**Setup**: Book with only Title, GenreId, StatusId  
**Expected**: Successful save with null optional fields  
**Result**: ? Passed

#### ? AddBookAsync_WithAllOptionalFields_Succeeds
**Purpose**: Verify all fields are saved correctly  
**Setup**: Book with all fields populated  
**Expected**: All fields persisted correctly  
**Result**: ? Passed

---

### 6. GetBookByIdAsync (4 tests)

#### ? GetBookByIdAsync_WithValidId_ReturnsBook
**Purpose**: Retrieve book by valid ID  
**Setup**: Book in database  
**Input**: Valid book ID  
**Expected**: Returns matching book  
**Result**: ? Passed

#### ? GetBookByIdAsync_IncludesRelatedEntities
**Purpose**: Verify eager loading of related data  
**Setup**: Book with author, genre, publisher, status  
**Expected**: All relations loaded (not null)  
**Result**: ? Passed

#### ? GetBookByIdAsync_WithInvalidId_ReturnsNull
**Purpose**: Handle non-existent ID gracefully  
**Input**: ID = 999 (doesn't exist)  
**Expected**: Returns null  
**Result**: ? Passed

#### ? GetBookByIdAsync_WithNullAuthorAndPublisher_HandlesCorrectly
**Purpose**: Handle optional relations correctly  
**Setup**: Book without author or publisher  
**Expected**: Genre and Status loaded, Author and Publisher null  
**Result**: ? Passed

---

### 7. UpdateBookAsync (4 tests)

#### ? UpdateBookAsync_UpdatesExistingBook
**Purpose**: Verify basic update functionality  
**Setup**: Book in database  
**Action**: Update title and year  
**Expected**: Changes persisted  
**Result**: ? Passed

#### ? UpdateBookAsync_UpdatesAllFields
**Purpose**: Verify all fields can be updated  
**Setup**: Book with all fields  
**Action**: Update every field  
**Expected**: All changes persisted  
**Result**: ? Passed

#### ? UpdateBookAsync_CanSetOptionalFieldsToNull
**Purpose**: Verify optional fields can be cleared  
**Setup**: Book with optional fields populated  
**Action**: Set AuthorId, PublisherId, Year, ISBN to null  
**Expected**: Fields set to null successfully  
**Result**: ? Passed

#### ? UpdateBookAsync_ChangeBookStatus_UpdatesCorrectly
**Purpose**: Verify book status can be changed  
**Setup**: Wishlist book  
**Action**: Change to Owned status  
**Expected**: Status updated successfully  
**Result**: ? Passed

---

## Test Infrastructure

### In-Memory Database
- Uses EF Core In-Memory provider
- Fresh database for each test (via `Guid.NewGuid()`)
- Isolated test execution

### Seed Data
Tests use pre-seeded reference data:

**Genres:**
- Fiction (ID: 1)
- Non-Fiction (ID: 2)
- Science Fiction (ID: 3)

**Authors:**
- Author One (ID: 1)
- Author Two (ID: 2)
- Author Three (ID: 3)

**Publishers:**
- Penguin (ID: 1)
- HarperCollins (ID: 2)
- Simon & Schuster (ID: 3)

**Book Statuses:**
- Owned (ID: 1)
- Wishlist (ID: 2)
- Read (ID: 3)

### Test Lifecycle
```csharp
[TestInitialize] ? Setup() ? Create DB + Seed data
[TestMethod] ? Execute test
[TestCleanup] ? Cleanup() ? Delete DB + Dispose
```

## Coverage Analysis

### Methods Covered: 7/7 (100%)
? GetOwnedBookCountAsync  
? GetWishlistBookCountAsync  
? GetOwnedBooksAsync  
? GetWishlistBooksAsync  
? AddBookAsync  
? GetBookByIdAsync  
? UpdateBookAsync  

### Scenarios Covered

| Scenario | Coverage |
|----------|----------|
| Happy path | ? Yes |
| Empty database | ? Yes |
| Null values | ? Yes |
| Filtering | ? Yes |
| Searching | ? Yes |
| Sorting | ? Yes |
| Edge cases | ? Yes |
| Invalid input | ? Yes |
| CRUD operations | ? Yes |
| Related entities | ? Yes |

## Running the Tests

### Run All Tests
```bash
dotnet test HomeLibrary.BusinessLogic.Tests
```

### Run with Detailed Output
```bash
dotnet test HomeLibrary.BusinessLogic.Tests --logger "console;verbosity=detailed"
```

### Run Specific Test
```bash
dotnet test --filter "FullyQualifiedName~GetOwnedBookCountAsync_ReturnsCorrectCount"
```

### Run Tests by Category
```bash
# Run all GetOwnedBooksAsync tests
dotnet test --filter "FullyQualifiedName~GetOwnedBooksAsync"
```

## Test Patterns Used

### AAA Pattern (Arrange-Act-Assert)
All tests follow the AAA pattern:
```csharp
// Arrange - Set up test data
var book = new Book { ... };

// Act - Execute the method
var result = await manager.GetBookByIdAsync(1);

// Assert - Verify the result
Assert.AreEqual(expected, result);
```

### Test Naming Convention
```
MethodName_Scenario_ExpectedBehavior
```
Examples:
- `GetOwnedBookCountAsync_ReturnsCorrectCount`
- `GetOwnedBooksAsync_WithSearchTerm_ReturnsMatchingBooks`
- `AddBookAsync_WithMinimalData_Succeeds`

## Benefits

### ? Comprehensive Coverage
- All public methods tested
- Multiple scenarios per method
- Edge cases covered

### ? Regression Protection
- Catch breaking changes early
- Verify bug fixes stay fixed
- Safe refactoring

### ? Documentation
- Tests serve as usage examples
- Clear expected behavior
- Living documentation

### ? Fast Execution
- In-memory database
- No external dependencies
- Runs in ~2 seconds

### ? Maintainable
- Clear test names
- Well-organized by method
- Reusable seed data

## Continuous Integration

These tests are designed to run in CI/CD pipelines:

```yaml
# Example GitHub Actions
- name: Run Unit Tests
  run: dotnet test --no-build --verbosity normal
```

```yaml
# Example Azure DevOps
- task: DotNetCoreCLI@2
  inputs:
    command: 'test'
    projects: '**/*Tests.csproj'
```

## Future Enhancements

Potential test improvements:
1. **Performance Tests**: Measure query performance with large datasets
2. **Concurrency Tests**: Test concurrent updates
3. **Integration Tests**: Test with real SQL Server
4. **Parameterized Tests**: Use `[DataRow]` for multiple inputs
5. **Mock Tests**: Test error handling with mocked exceptions
6. **Code Coverage**: Generate coverage reports (aim for >90%)

## Related Files

- **Implementation**: `HomeLibrary.BusinessLogic/Managers/BookManager.cs`
- **Tests**: `HomeLibrary.BusinessLogic.Tests/BookManagerTests.cs`
- **Interface**: `HomeLibrary.Core/Interfaces/IBookManager.cs`
- **Models**: `HomeLibrary.Core/Models/Book.cs`
- **DTOs**: `HomeLibrary.Core/DataTransferObjects/BookDto.cs`

## Conclusion

This comprehensive test suite provides:
- ? 100% method coverage
- ? 34 passing tests
- ? Robust validation of all scenarios
- ? Fast execution time
- ? Easy to maintain and extend

The tests ensure the `BookManager` class works correctly in all scenarios and provides confidence for future changes.
