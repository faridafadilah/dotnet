using TodoApi.Models;

namespace TodoApi.Data.Interface
{
  public interface ITodoRepository
  {
    (IEnumerable<TodoItem> todos, int totalCount) getAllTodos(int pageNumber, int pageSize, string search);
    TodoItem getById(long id);
    void createTodo(TodoItem todo);
    bool checkDuplicate(string name);
    bool saveChanges();
    void updateTodo(TodoItem todo);
    void deleteTodoById(long id);
  }
}