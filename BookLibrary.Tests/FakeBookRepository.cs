namespace BookLibrary.Tests
{
    public sealed class FakeBookRepository : IBookRepository
    {
        public List<Book> Load(string filePath)
        {
            return new List<Book>();
        }

        public void Save(List<Book> books, string filePath)
        {
        }
    }
}