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
}