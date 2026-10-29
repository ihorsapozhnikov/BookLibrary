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
}