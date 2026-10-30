namespace BookLibrary.Tests;

public class BookLibraryServiceTests
{
    private readonly FakeBookRepository _repository;
    private readonly BookLibraryService _service;

    public BookLibraryServiceTests()
    {
        _repository = new FakeBookRepository();
        _service = new BookLibraryService(_repository, "books.xml");
    }

    #region AddBook Tests

    [Fact]
    public void AddBook_ShouldAddBookToLibrary()
    {
        var book = new Book
        {
            Title = "The Green Mile",
            Author = "Stephen King",
            PageCount = 1000
        };

        _service.AddBook(book);

        var books = _service.GetBooks();

        Assert.Single(books);
        Assert.Equal("The Green Mile", books[0].Title);
        Assert.Equal("Stephen King", books[0].Author);
        Assert.Equal(1000, books[0].PageCount);
    }

    [Fact]
    public void AddBook_ShouldAddMultipleBooksToLibrary()
    {
        var firstBook = new Book
        {
            Title = "The Green Mile",
            Author = "Stephen King",
            PageCount = 1000
        };

        var secondBook = new Book
        {
            Title = "The Little Mermaid",
            Author = "Hans Christian Andersen",
            PageCount = 100
        };

        _service.AddBook(firstBook);
        _service.AddBook(secondBook);

        var books = _service.GetBooks();

        Assert.Equal(2, books.Count);
        Assert.Equal("The Green Mile", books[0].Title);
        Assert.Equal("The Little Mermaid", books[1].Title);
    }

    [Fact]
    public void AddBook_ShouldThrowArgumentNullException_WhenBookIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _service.AddBook(null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void AddBook_ShouldThrowArgumentException_WhenTitleIsEmptyOrWhitespace(string title)
    {
        var book = new Book
        {
            Title = title,
            Author = "Stephen King",
            PageCount = 1000
        };

        Assert.Throws<ArgumentException>(() => _service.AddBook(book));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void AddBook_ShouldThrowArgumentException_WhenAuthorIsEmptyOrWhitespace(string author)
    {
        var book = new Book
        {
            Title = "The Green Mile",
            Author = author,
            PageCount = 1000
        };

        Assert.Throws<ArgumentException>(() => _service.AddBook(book));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AddBook_ShouldThrowArgumentOutOfRangeException_WhenPageCountIsNotPositive(int pageCount)
    {
        var book = new Book
        {
            Title = "The Green Mile",
            Author = "Stephen King",
            PageCount = pageCount
        };

        Assert.Throws<ArgumentOutOfRangeException>(() => _service.AddBook(book));
    }
    #endregion

    #region GetSortedBooks Tests

    [Fact]
    public void GetSortedBooks_ShouldSortBooksByAuthorAndTitle()
    {
        _service.AddBook(new Book
        {
            Title = "The Green Mile",
            Author = "Stephen King",
            PageCount = 1000
        });

        _service.AddBook(new Book
        {
            Title = "The Little Mermaid",
            Author = "Hans Christian Andersen",
            PageCount = 100
        });

        _service.AddBook(new Book
        {
            Title = "It",
            Author = "Stephen King",
            PageCount = 2000
        });

        var books = _service.GetSortedBooks();

        Assert.Equal(["Hans Christian Andersen", "Stephen King", "Stephen King"], books.Select(book => book.Author));
        Assert.Equal(["The Little Mermaid", "It", "The Green Mile"], books.Select(book => book.Title));
    }

    [Fact]
    public void GetSortedBooks_ShouldNotChangeOriginalOrder()
    {
        _service.AddBook(new Book
        {
            Title = "The Green Mile",
            Author = "Stephen King",
            PageCount = 2000
        });

        _service.AddBook(new Book
        {
            Title = "It",
            Author = "Stephen King",
            PageCount = 1000
        });

        var sortedBooks = _service.GetSortedBooks();
        var books = _service.GetBooks();

        Assert.Equal(["The Green Mile", "It"], books.Select(book => book.Title));
        Assert.Equal(["It", "The Green Mile"], sortedBooks.Select(book => book.Title));
    }

    #endregion

    #region SearchByTitle Tests

    [Fact]
    public void SearchByTitle_ShouldReturnBooks_WhenTitleContainsSearchText()
    {
        _service.AddBook(new Book
        {
            Title = "The Green Mile",
            Author = "Stephen King",
            PageCount = 2000
        });

        _service.AddBook(new Book
        {
            Title = "It",
            Author = "Stephen King",
            PageCount = 1000
        });

        var result = _service.SearchByTitle("Green");

        var book = Assert.Single(result);
        Assert.Equal("The Green Mile", book.Title);
    }

    [Fact]
    public void SearchByTitle_ShouldReturnEmptyList_WhenNoBooksMatch()
    {
        _service.AddBook(new Book
        {
            Title = "The Green Mile",
            Author = "Stephen King",
            PageCount = 2000
        });

        var result = _service.SearchByTitle("Harry");

        Assert.Empty(result);
    }

    [Fact]
    public void SearchByTitle_ShouldIgnoreCase_WhenSearchingByTitle()
    {
        _service.AddBook(new Book
        {
            Title = "The Green Mile",
            Author = "Stephen King",
            PageCount = 2000
        });

        var result = _service.SearchByTitle("green");

        var book = Assert.Single(result);
        Assert.Equal("The Green Mile", book.Title);
    }

    [Fact]
    public void SearchByTitle_ShouldReturnAllMatchingBooks_WhenMultipleBooksMatch()
    {
        _service.AddBook(new Book
        {
            Title = "The Green Mile",
            Author = "Stephen King",
            PageCount = 2000
        });

        _service.AddBook(new Book
        {
            Title = "Green River",
            Author = "John Smith",
            PageCount = 500
        });

        _service.AddBook(new Book
        {
            Title = "It",
            Author = "Stephen King",
            PageCount = 1000
        });

        var result = _service.SearchByTitle("Green");

        Assert.Equal(2, result.Count);
        Assert.Contains(result, book => book.Title == "The Green Mile");
        Assert.Contains(result, book => book.Title == "Green River");
    }

    [Fact]
    public void SearchByTitle_ShouldReturnBook_WhenSearchTextMatchesFullTitle()
    {
        _service.AddBook(new Book
        {
            Title = "The Green Mile",
            Author = "Stephen King",
            PageCount = 2000
        });

        var result = _service.SearchByTitle("The Green Mile");

        var book = Assert.Single(result);
        Assert.Equal("The Green Mile", book.Title);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void SearchByTitle_ShouldThrowArgumentException_WhenSearchTextIsEmptyOrWhitespace(string searchText)
    {
        Assert.Throws<ArgumentException>(() => _service.SearchByTitle(searchText));
    }

    [Fact]
    public void SearchByTitle_ShouldNotSearchByAuthor()
    {
        _service.AddBook(new Book
        {
            Title = "The Green Mile",
            Author = "Stephen King",
            PageCount = 2000
        });

        var result = _service.SearchByTitle("Stephen King");

        Assert.Empty(result);
    }

    #endregion

    #region LoadBooks Tests

    [Fact]
    public void LoadBooks_ShouldLoadBooksFromRepository()
    {
        _repository.BooksToLoad.Add(new Book
        {
            Title = "The Green Mile",
            Author = "Stephen King",
            PageCount = 1000
        });

        _service.LoadBooks();

        var books = _service.GetBooks();

        var book = Assert.Single(books);
        Assert.Equal("The Green Mile", book.Title);
        Assert.Equal("Stephen King", book.Author);
        Assert.Equal(1000, book.PageCount);
        Assert.Equal("books.xml", _repository.LoadedFilePath);
    }

    [Fact]
    public void LoadBooks_ShouldReplaceExistingBooks()
    {
        _service.AddBook(new Book
        {
            Title = "The Green Mile",
            Author = "Stephen King",
            PageCount = 1000
        });

        _repository.BooksToLoad.Add(new Book
        {
            Title = "It",
            Author = "Stephen King",
            PageCount = 2000
        });

        _service.LoadBooks();

        var books = _service.GetBooks();

        var book = Assert.Single(books);
        Assert.Equal("It", book.Title);
        Assert.Equal("Stephen King", book.Author);
        Assert.Equal(2000, book.PageCount);
    }

    #endregion

    #region SaveBooks Tests

    [Fact]
    public void SaveBooks_ShouldSaveBooksToRepository()
    {
        _service.AddBook(new Book
        {
            Title = "The Green Mile",
            Author = "Stephen King",
            PageCount = 1000
        });

        _service.SaveBooks();

        var book = Assert.Single(_repository.SavedBooks!);

        Assert.Equal("The Green Mile", book.Title);
        Assert.Equal("Stephen King", book.Author);
        Assert.Equal(1000, book.PageCount);
        Assert.Equal("books.xml", _repository.SavedFilePath);
    }

    #endregion
}