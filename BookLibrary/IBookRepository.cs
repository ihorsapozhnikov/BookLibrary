namespace BookLibrary
{
    public interface IBookRepository
    {
        List<Book> Load(string filePath);
        void Save(List<Book> books, string filePath);
    }
}
