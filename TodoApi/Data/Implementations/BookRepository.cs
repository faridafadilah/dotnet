using TodoApi.Models;
using TodoApi.Data.Interface;

namespace TodoApi.Data.Implementations
{

  public class BookRepository : IBookRepository
  {
    private readonly TodoContext _context;

    public BookRepository(TodoContext _context)
    {
      this._context = _context;
    }

    public IEnumerable<Book> getAllBooks()
    {
      var books = _context.Books.ToList();
      return books;
    }

    public Book getById(long id)
    {
      var book = _context.Books.Find(id);
      if (book == null)
      {
        return null; // or throw an exception, depending on the desired behavior
      }
      return book;
    }

    public bool checkDuplicate(String name)
    {
      var check = _context.Books.FirstOrDefault(x => x.Name.Equals(name));
      if (check == null)
      {
        return false;
      }
      return true;
    }

    public void createBook(Book book)
    {
      // var check = _context.Books.Any(x => x.Name == book.Name);
      // if (check)
      // {
      //   throw new AppException("Menu name is already taken");
      // }
      if (book == null)
      {
        throw new ArgumentNullException(nameof(book));
      }

      _context.Books.Add(book);
    }

    public bool saveChanges()
    {
      return (_context.SaveChanges() >= 0);
    }

    public void updateBook(Book book)
    {
    }

    public void deleteBookById(long id)
    {
      var book = _context.Books.Find(id);
      if (book == null)
      {
        throw new ArgumentNullException(nameof(book));
      }
      else
      {
        _context.Books.Remove(book);
        _context.SaveChanges();
      }

    }
  }

}