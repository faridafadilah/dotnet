using TodoApi.Models;

namespace TodoApi.Data.Interface
{
  public interface IBookRepository
  {
    (IEnumerable<Book> books, int totalCount) getAllBooks(int pageNumber, int pageSize, string search);
    Book getById(long id);
    void createBook(Book book);
    bool checkDuplicate(string name);
    bool saveChanges();
    void updateBook(Book book);
    void deleteBookById(long id);
  }
}