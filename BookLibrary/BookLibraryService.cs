namespace BookLibrary;

public class BookLibraryService
{
    private readonly IBookRepository _repository;
    private readonly string _filePath;
    private List<Book> _books = new();

    public BookLibraryService(IBookRepository repository, string filePath)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        _repository = repository;
        _filePath = filePath;
    }

    public void LoadBooks()
    {
        _books = _repository.Load(_filePath);
    }

    public void AddBook(Book book)
    {
        ArgumentNullException.ThrowIfNull(book);
        _books.Add(book);
    }

    public List<Book> GetBooks()
    {
        return _books.ToList();
    }

    public List<Book> GetSortedBooks()
    {
        return _books
            .OrderBy(book => book.Author, StringComparer.OrdinalIgnoreCase)
            .ThenBy(book => book.Title, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public List<Book> SearchByTitle(string searchText)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(searchText);

        return _books.Where(book => book.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public void SaveBooks()
    {
        _repository.Save(_books, _filePath);
    }
}