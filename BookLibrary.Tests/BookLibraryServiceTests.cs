namespace BookLibrary.Tests;

public class BookLibraryServiceTests
{
    [Fact]
    public void AddBook_ShouldAddBookToLibrary()
    {
        var repository = new FakeBookRepository();
        var service = new BookLibraryService(repository, "books.xml");

        var book = new Book
        {
            Title = "The Green Mile",
            Author = "Stephen King",
            PageCount = 1000
        };

        service.AddBook(book);

        var books = service.GetBooks();

        Assert.Single(books);
        Assert.Equal("The Green Mile", books[0].Title);
        Assert.Equal("Stephen King", books[0].Author);
        Assert.Equal(1000, books[0].PageCount);
    }


    [Fact]
    public void GetSortedBooks_ShouldSortBooksByAuthorAndTitle()
    {
        var repository = new FakeBookRepository();
        var service = new BookLibraryService(repository, "books.xml");

        service.AddBook(new Book
        {
            Title = "The Green Mile",
            Author = "Stephen King",
            PageCount = 1000
        });

        service.AddBook(new Book
        {
            Title = "The Little Mermaid",
            Author = "Hans Christian Andersen",
            PageCount = 100
        });

        service.AddBook(new Book
        {
            Title = "It",
            Author = "Stephen King",
            PageCount = 2000
        });

        var books = service.GetSortedBooks();

        Assert.Equal(["Hans Christian Andersen", "Stephen King", "Stephen King"], books.Select(book => book.Author));
        Assert.Equal(["The Little Mermaid", "It", "The Green Mile"], books.Select(book => book.Title));
    }

    [Fact]
    public void GetSortedBooks_ShouldNotChangeOriginalOrder()
    {
        var repository = new FakeBookRepository();
        var service = new BookLibraryService(repository, "books.xml");

        service.AddBook(new Book
        {
            Title = "The Green Mile",
            Author = "Stephen King",
            PageCount = 2000
        });

        service.AddBook(new Book
        {
            Title = "It",
            Author = "Stephen King",
            PageCount = 1000
        });

        var sortedBooks = service.GetSortedBooks();
        var books = service.GetBooks();

        Assert.Equal(["The Green Mile", "It"], books.Select(book => book.Title));
        Assert.Equal(["It", "The Green Mile"], sortedBooks.Select(book => book.Title));
    }

    [Fact]
    public void SearchByTitle_ShouldReturnBooks_WhenTitleContainsSearchText()
    {
        var repository = new FakeBookRepository();
        var service = new BookLibraryService(repository, "books.xml");

        service.AddBook(new Book
        {
            Title = "The Green Mile",
            Author = "Stephen King",
            PageCount = 2000
        });

        service.AddBook(new Book
        {
            Title = "It",
            Author = "Stephen King",
            PageCount = 1000
        });

        var result = service.SearchByTitle("Green");

        var book = Assert.Single(result);
        Assert.Equal("The Green Mile", book.Title);
    }

    [Fact]
    public void SearchByTitle_ShouldReturnEmptyList_WhenNoBooksMatch()
    {
        var repository = new FakeBookRepository();
        var service = new BookLibraryService(repository, "books.xml");

        service.AddBook(new Book
        {
            Title = "The Green Mile",
            Author = "Stephen King",
            PageCount = 2000
        });

        var result = service.SearchByTitle("Harry");

        Assert.Empty(result);
    }

    [Fact]
    public void SearchByTitle_ShouldIgnoreCase_WhenSearchingByTitle()
    {
        var repository = new FakeBookRepository();
        var service = new BookLibraryService(repository, "books.xml");

        service.AddBook(new Book
        {
            Title = "The Green Mile",
            Author = "Stephen King",
            PageCount = 2000
        });

        var result = service.SearchByTitle("green");

        var book = Assert.Single(result);
        Assert.Equal("The Green Mile", book.Title);
    }

    [Fact]
    public void SearchByTitle_ShouldReturnAllMatchingBooks_WhenMultipleBooksMatch()
    {
        var repository = new FakeBookRepository();
        var service = new BookLibraryService(repository, "books.xml");

        service.AddBook(new Book
        {
            Title = "The Green Mile",
            Author = "Stephen King",
            PageCount = 2000
        });

        service.AddBook(new Book
        {
            Title = "Green River",
            Author = "John Smith",
            PageCount = 500
        });

        service.AddBook(new Book
        {
            Title = "It",
            Author = "Stephen King",
            PageCount = 1000
        });

        var result = service.SearchByTitle("Green");

        Assert.Equal(2, result.Count);
        Assert.Contains(result, book => book.Title == "The Green Mile");
        Assert.Contains(result, book => book.Title == "Green River");
    }

    [Fact]
    public void SearchByTitle_ShouldReturnBook_WhenSearchTextMatchesFullTitle()
    {
        var repository = new FakeBookRepository();
        var service = new BookLibraryService(repository, "books.xml");

        service.AddBook(new Book
        {
            Title = "The Green Mile",
            Author = "Stephen King",
            PageCount = 2000
        });

        var result = service.SearchByTitle("The Green Mile");

        var book = Assert.Single(result);
        Assert.Equal("The Green Mile", book.Title);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void SearchByTitle_ShouldThrowArgumentException_WhenSearchTextIsEmptyOrWhitespace(string searchText)
    {
        var repository = new FakeBookRepository();
        var service = new BookLibraryService(repository, "books.xml");

        Assert.Throws<ArgumentException>(() => service.SearchByTitle(searchText));
    }

    [Fact]
    public void SearchByTitle_ShouldNotSearchByAuthor()
    {
        var repository = new FakeBookRepository();
        var service = new BookLibraryService(repository, "books.xml");

        service.AddBook(new Book
        {
            Title = "The Green Mile",
            Author = "Stephen King",
            PageCount = 2000
        });

        var result = service.SearchByTitle("Stephen King");

        Assert.Empty(result);
    }
}