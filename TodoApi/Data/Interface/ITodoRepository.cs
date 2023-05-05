using TodoApi.Models;

namespace TodoApi.Data.Interface
{
  public interface ITodoRepository
  {
    IEnumerable<TodoItem> getAllTodos();
    TodoItem getById(long id);
    void createTodo(TodoItem todo);
    bool checkDuplicate(string name);
    bool saveChanges();
    void updateTodo(TodoItem todo);
    void deleteTodoById(long id);
  }
}