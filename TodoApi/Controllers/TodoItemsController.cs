using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TodoApi.Data.Interface;
using TodoApi.Models;
using TodoApi.Dtos;
using TodoApi.Base;

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
    // [Authorize(Roles = "super, user")]
    public ActionResult<BasePageResponse<IEnumerable<ResTodoDTO>>> GetTodoItems([FromQuery(Name = "page")] int pageNumber = 1,
    [FromQuery(Name = "limit")] int pageSize = 10, [FromQuery(Name = "search")] string? search = null)
    {
      var (todoItems, totalCount) = repository.getAllTodos(pageNumber, pageSize, search);
      if (todoItems != null && todoItems.Any())
      {
        var mappedData = mapper.Map<IEnumerable<ResTodoDTO>>(todoItems);
        BasePageResponse<IEnumerable<ResTodoDTO>> response = new BasePageResponse<IEnumerable<ResTodoDTO>>();
        response.code = 200;
        response.message = "Success";
        response.currentPage = pageNumber;
        response.data = mappedData;
        response.totalPage = (int)Math.Ceiling((double)totalCount / pageSize);
        response.totalElement = totalCount;
        return Ok(response);
      }
      else
      {
        return NotFound(new ResponApi<IEnumerable<ResTodoDTO>>
        {
          message = "No data found",
          code = 404,
          data = null
        });
      }
    }

    // GET: api/TodoItems/5
    [HttpGet("{id}")]
    public ActionResult<ResponApi<ResTodoDTO>> GetTodoItem(long id)
    {
      ResponApi<ResTodoDTO> response = new ResponApi<ResTodoDTO>();
      var todoItem = repository.getById(id);

      if (todoItem == null)
      {
        response.code = 404;
        response.message = "Not Found by Id=" + id;
        response.data = null;
        return NotFound(response);
      }
      var mappedData = mapper.Map<ResTodoDTO>(todoItem);
      response.code = 200;
      response.message = "Success Get By Id";
      response.data = mappedData;

      return Ok(response);
    }

    // PUT: api/TodoItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public ActionResult<ResponApi<TodoItem>> PutTodoItem(long id, [FromForm] TodoItemDTO todoItem)
    {
      ResponApi<TodoItem> response = new ResponApi<TodoItem>();
      var modelDto = repository.getById(id);
      if (modelDto == null)
      {
        response.code = 404;
        response.message = "Not Found by Id=" + id;
        response.data = null;
        return NotFound(response);
      }

      if (todoItem.File != null && todoItem.File.Length > 0)
      {
        // Jika ada file yang diupload, simpan file dan nama filenya ke dalam database
        string fileName = Path.GetFileName(todoItem.File.FileName);
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
          todoItem.File.CopyTo(fileStream);
        }
        modelDto.FileName = fileName;
        modelDto.FilePath = filePath;
      }

      mapper.Map(todoItem, modelDto);
      repository.updateTodo(modelDto);
      repository.saveChanges();

      response.code = 200;
      response.message = "Success Get By Id";
      response.data = modelDto;
      return Ok(response);
    }

    //For File Upload
    [HttpPost("upload")]
    public async Task<ActionResult<string>> UploadFile([FromForm] IFormFile file)
    {
      if (file == null || file.Length == 0)
      {
        return BadRequest("Please select a file");
      }

      var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
      var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName);

      using (var stream = new FileStream(filePath, FileMode.Create))
      {
        await file.CopyToAsync(stream);
      }

      return Ok(fileName);
    }

    // POST: api/TodoItems
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost()]
    public async Task<ActionResult<ResponApi<TodoItemDTO>>> PostTodoItem([FromForm] TodoItemDTO todoItem)
    {
      ResponApi<TodoItemDTO> response = new ResponApi<TodoItemDTO>();
      var checkDuplicate = repository.checkDuplicate(todoItem.Name);
      if (checkDuplicate)
      {
        return BadRequest("Data Todo sudah tersedia!");
      }

      var todo = new TodoItem
      {
        Id = todoItem.Id,
        Name = todoItem.Name,
        IsComplete = todoItem.IsComplete
      };

      if (todoItem.File != null)
      {
        using (var client = new HttpClient())
        {
          using (var formData = new MultipartFormDataContent())
          {
            formData.Add(new StreamContent(todoItem.File.OpenReadStream()), "file", todoItem.File.FileName);

            var responses = await client.PostAsync("http://localhost:5162/api/TodoItems/upload", formData);

            if (responses.IsSuccessStatusCode)
            {
              var fileName = await responses.Content.ReadAsStringAsync();
              todo.FileName = fileName;
              todo.FilePath = fileName;
              todo.File = todoItem.File;
            }
          }
        }
      }

      repository.createTodo(todo);
      repository.saveChanges();
      var mappedData = mapper.Map<TodoItemDTO>(todo);
      response.code = 200;
      response.message = "Success Get By Id";
      response.data = mappedData;
      return Ok(response);
    }

    // DELETE: api/TodoItems/5
    [HttpDelete("{id}")]
    public ActionResult<ResponApi<ResTodoDTO>> DeleteTodoItem(long id)
    {
      ResponApi<ResTodoDTO> response = new ResponApi<ResTodoDTO>();
      var todoItem = repository.getById(id);
      if (todoItem == null)
      {
        response.code = 404;
        response.message = "Data Not Found by Id=" + id;
        response.data = null;
        return NotFound(response);
      }

      repository.deleteTodoById(id);
      repository.saveChanges();

      response.code = 200;
      response.message = "Success Delete by Id=" + id;
      response.data = null;

      return Ok(response);
    }
  }
}