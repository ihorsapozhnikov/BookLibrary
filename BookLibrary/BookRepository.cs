using System.Xml.Serialization;

namespace BookLibrary;

public class BookRepository : IBookRepository
{
    public List<Book> Load(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("XML file was not found.", filePath);
        }

        var serializer = CreateSerializer();

        using var stream = File.OpenRead(filePath);
        var result = serializer.Deserialize(stream);

        if (result is not List<Book> books)
        {
            throw new InvalidOperationException("Failed to deserialize books from XML.");
        }

        if (books.Any(book =>
            string.IsNullOrWhiteSpace(book.Title) ||
            string.IsNullOrWhiteSpace(book.Author) ||
            book.PageCount <= 0))
        {
            throw new InvalidDataException("XML contains invalid book data.");
        }

        return books;
    }

    public void Save(List<Book> books, string filePath)
    {
        ArgumentNullException.ThrowIfNull(books);
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        var serializer = CreateSerializer();

        using var stream = File.Create(filePath);
        serializer.Serialize(stream, books);
    }

    private static XmlSerializer CreateSerializer()
    {
        return new XmlSerializer(typeof(List<Book>), new XmlRootAttribute("Books"));
    }
}
