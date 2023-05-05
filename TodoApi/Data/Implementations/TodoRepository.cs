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

    public IEnumerable<TodoItem> getAllTodos()
    {
      var todos = _context.TodoItems.ToList();
      foreach (var todo in todos)
      {
        todo.Books = _context.Books.Where(x => x.Todo.Id == todo.Id).ToList();
      }
      return todos;
    }

    public TodoItem getById(long id)
    {
      var todo = _context.TodoItems.Find(id);
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