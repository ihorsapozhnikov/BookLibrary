namespace BookLibrary.Tests
{
    public sealed class FakeBookRepository : IBookRepository
    {
        public List<Book> BooksToLoad { get; set; } = new();
        public List<Book>? SavedBooks { get; private set; }
        public string? LoadedFilePath { get; private set; }
        public string? SavedFilePath { get; private set; }

        public List<Book> Load(string filePath)
        {
            LoadedFilePath = filePath;
            return BooksToLoad.ToList();
        }

        public void Save(List<Book> books, string filePath)
        {
            SavedBooks = books.ToList();
            SavedFilePath = filePath;
        }
    }
}