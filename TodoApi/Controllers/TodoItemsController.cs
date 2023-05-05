using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TodoApi.Data.Interface;
using TodoApi.Models;
using TodoApi.Dtos;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ITodoRepository repository;

        public TodoItemsController(ITodoRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

    // GET: api/TodoItems
    [HttpGet]
    [Authorize(Roles = "super, user")]
    public ActionResult<IEnumerable<ResTodoDTO>> GetTodoItems()
    {
      var todoItems = repository.getAllTodos();
      if (todoItems != null)
      {
        var mappedData = mapper.Map<IEnumerable<ResTodoDTO>>(todoItems);
        return Ok(mappedData);
      }
      return NotFound();
    }

    // GET: api/TodoItems/5
    [HttpGet("{id}")]
    public ActionResult<ResTodoDTO> GetTodoItem(long id)
    {
        var todoItem = repository.getById(id);

        if (todoItem == null)
        {
            return NotFound();
        }
        var mappedData = mapper.Map<ResTodoDTO>(todoItem);

        return Ok(mappedData);
    }

    // PUT: api/TodoItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public IActionResult PutTodoItem(long id, TodoItemDTO todoItem)
    {
        var modelDto = repository.getById(id);
        if(modelDto == null) {
          return NotFound("Data Not Found");
        }
        mapper.Map(todoItem, modelDto);
        repository.updateTodo(modelDto);
        repository.saveChanges();
        return Ok("Berhasil Update!");
    }

    // POST: api/TodoItems
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost()]
    public ActionResult<TodoItemDTO> PostTodoItem(TodoItemDTO todoItem)
    {
        var checkDuplicate = repository.checkDuplicate(todoItem.Name);
        if(checkDuplicate) {
          return BadRequest("Data Todo sudah tersedia!");
        }

        var todo = new TodoItem {
          Id = todoItem.Id,
          Name = todoItem.Name,
          IsComplete = todoItem.IsComplete
        };

        repository.createTodo(todo);
        repository.saveChanges();
        var mappedData = mapper.Map<TodoItemDTO>(todo);
        return Ok(mappedData);
    }

    // DELETE: api/TodoItems/5
    [HttpDelete("{id}")]
    public IActionResult DeleteTodoItem(long id)
    {
        var todoItem = repository.getById(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        repository.deleteTodoById(id);
        repository.saveChanges();
        return Ok("Berhasil dihapus!");
    }
        }
    }