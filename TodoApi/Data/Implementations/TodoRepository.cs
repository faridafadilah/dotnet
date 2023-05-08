using TodoApi.Models;
using TodoApi.Data.Interface;

namespace TodoApi.Data.Implementations
{

  public class TodoRepository : ITodoRepository
  {
    private readonly TodoContext _context;

    public TodoRepository(TodoContext _context)
    {
      this._context = _context;
    }

    public IEnumerable<TodoItem> getAllTodos(int pageNumber, int pageSize, string search)
    {
      IQueryable<TodoItem> query = _context.TodoItems;
      if (!string.IsNullOrEmpty(search))
      {
        query = query.Where(t => t.Name.Contains(search));
      }
      query = query.OrderByDescending(t => t.CreatedAt);

      int totalCount = query.Count();
      int totalPage = (int)Math.Ceiling((double)totalCount / pageSize);

      var todos = query
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToList();

      foreach (var todo in todos)
      {
        todo.Books = _context.Books.Where(x => x.Todo.Id == todo.Id).ToList();
      }
      return todos;
    }

    public TodoItem getById(long id)
    {
      var todo = _context.TodoItems.Find(id);
      if (todo == null)
      {
        return null; // or throw an exception, depending on the desired behavior
      }
      todo.Books = _context.Books.Where(x => x.Todo.Id == todo.Id).ToList();
      return todo;
    }

    public bool checkDuplicate(String name)
    {
      var check = _context.TodoItems.FirstOrDefault(x => x.Name.Equals(name));
      if (check == null)
      {
        return false;
      }
      return true;
    }

    public void createTodo(TodoItem todo)
    {
      // var check = _context.TodoItems.Any(x => x.Name == todo.Name);
      // if (check)
      // {
      //   throw new AppException("Menu name is already taken");
      // }
      if (todo == null)
      {
        throw new ArgumentNullException(nameof(todo));
      }

      if (todo.File != null)
      {
        todo.FileName = todo.File.FileName;
      }

      _context.TodoItems.Add(todo);
    }

    public bool saveChanges()
    {
      return (_context.SaveChanges() >= 0);
    }

    public void updateTodo(TodoItem todo)
    {
    }

    public void deleteTodoById(long id)
    {
      var todo = _context.TodoItems.Find(id);
      if (todo == null)
      {
        throw new ArgumentNullException(nameof(todo));
      }
      else
      {
        _context.TodoItems.Remove(todo);
        _context.SaveChanges();
      }

    }
  }

}