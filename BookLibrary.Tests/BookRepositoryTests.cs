namespace BookLibrary.Tests
{
    public class BookRepositoryTests
    {
        private readonly BookRepository _repository = new();

        [Fact]
        public void Load_ShouldReturnBooks_WhenXmlIsValid()
        {
            var filePath = Path.GetTempFileName();

            try
            {
                File.WriteAllText(filePath, """
                <?xml version="1.0" encoding="utf-8"?>
                <Books>
                  <Book>
                    <Title>The Green Mile</Title>
                    <Author>Stephen King</Author>
                    <PageCount>1000</PageCount>
                  </Book>
                  <Book>
                    <Title>It</Title>
                    <Author>Stephen King</Author>
                    <PageCount>2000</PageCount>
                  </Book>
                </Books>
                """);

                var books = _repository.Load(filePath);

                Assert.Equal(2, books.Count);

                Assert.Equal("The Green Mile", books[0].Title);
                Assert.Equal("Stephen King", books[0].Author);
                Assert.Equal(1000, books[0].PageCount);

                Assert.Equal("It", books[1].Title);
                Assert.Equal(2000, books[1].PageCount);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [Fact]
        public void Load_ShouldReturnEmptyList_WhenXmlContainsNoBooks()
        {
            var filePath = Path.GetTempFileName();

            try
            {
                File.WriteAllText(filePath, "<Books />");
                var books = _repository.Load(filePath);
                Assert.Empty(books);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [Fact]
        public void Load_ShouldThrowFileNotFoundException_WhenFileDoesNotExist()
        {
            var filePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            Assert.Throws<FileNotFoundException>(() => _repository.Load(filePath));
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        public void Load_ShouldThrowArgumentException_WhenFilePathIsEmptyOrWhitespace(string filePath)
        {
            Assert.Throws<ArgumentException>(() => _repository.Load(filePath));
        }

        [Fact]
        public void Save_ShouldSaveBooksToXmlFile()
        {
            var filePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            var books = new List<Book>
            {
                new()
                {
                    Title = "The Green Mile",
                    Author = "Stephen King",
                    PageCount = 1000
                },
                new()
                {
                    Title = "It",
                    Author = "Stephen King",
                    PageCount = 2000
                }
            };

            try
            {
                _repository.Save(books, filePath);

                Assert.True(File.Exists(filePath));

                var loadedBooks = _repository.Load(filePath);

                Assert.Equal(2, loadedBooks.Count);

                Assert.Equal("The Green Mile", loadedBooks[0].Title);
                Assert.Equal("Stephen King", loadedBooks[0].Author);
                Assert.Equal(1000, loadedBooks[0].PageCount);

                Assert.Equal("It", loadedBooks[1].Title);
                Assert.Equal(2000, loadedBooks[1].PageCount);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [Fact]
        public void Save_ShouldSaveEmptyList()
        {
            var filePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            try
            {
                _repository.Save(new List<Book>(), filePath);
                var loadedBooks = _repository.Load(filePath);
                Assert.Empty(loadedBooks);
            }
            finally
            {
                File.Delete(filePath);
            }
        }
    }
}
