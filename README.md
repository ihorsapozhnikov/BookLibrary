# BookLibrary

A simple C# library for managing a collection of books.

The library allows other applications to load books from an XML file, add new books, search by title, sort the collection and save changes back to XML.

## Features

- Load books from an XML file.
- Add new books to the collection.
- Get the list of books.
- Sort books alphabetically by author and then by title.
- Search books by part of the title (case-insensitive).
- Save the collection to an XML file.

## Book structure

Each book contains three properties:

- `Title` – book title.
- `Author` – author's name.
- `PageCount` – number of pages.

## XML format

The library uses XML to store books. The root element is `Books`, and each book is represented by a `Book` element.

Example:

```xml
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
```

## Requirements

- .NET 10 SDK

## Tests

The project uses xUnit for unit testing.
