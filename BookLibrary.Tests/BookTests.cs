namespace BookLibrary.Tests;

public class BookTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        var book = new Book();

        Assert.Equal(string.Empty, book.Title);
        Assert.Equal(string.Empty, book.Author);
        Assert.Equal(0, book.PageCount);
    }

    [Fact]
    public void Properties_ShouldStoreAssignedValues()
    {
        var book = new Book
        {
            Title = "The Green Mile",
            Author = "Stephen King",
            PageCount = 1000
        };

        Assert.Equal("The Green Mile", book.Title);
        Assert.Equal("Stephen King", book.Author);
        Assert.Equal(1000, book.PageCount);
    }
}
