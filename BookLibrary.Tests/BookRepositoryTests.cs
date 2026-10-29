using System;
using System.Collections.Generic;
using System.Text;

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

    }
}
