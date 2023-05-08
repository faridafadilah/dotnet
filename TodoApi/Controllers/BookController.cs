using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TodoApi.Data.Interface;
using TodoApi.Models;
using TodoApi.Dtos;
using TodoApi.Base;

namespace TodoApi.Controllers
{
  [Route("api/books")]
  [ApiController]
  public class BookController : ControllerBase
  {
    private readonly IMapper mapper;
    private readonly IBookRepository repository;
    private readonly ITodoRepository repositoryTodo;

    public BookController(IBookRepository repository, IMapper mapper, ITodoRepository repositoryTodo)
    {
      this.repository = repository;
      this.mapper = mapper;
      this.repositoryTodo = repositoryTodo;
    }

    // GET: api/TodoItems
    [HttpGet]
    public ActionResult<ResponApi<IEnumerable<BookDTO>>> GetBooks()
    {
      var books = repository.getAllBooks();
      if (books != null)
      {
        var mappedData = mapper.Map<IEnumerable<BookDTO>>(books);
        return Ok(new ResponApi<IEnumerable<BookDTO>>
        {
          message = "Success",
          code = 200,
          data = mappedData
        });
      }
      return NotFound(new ResponApi<IEnumerable<BookDTO>>
      {
        message = "No data found",
        code = 404,
        data = null
      });
    }

    // GET: api/TodoItems/5
    [HttpGet("{id}")]
    public ActionResult<ResponApi<BookDTO>> GetBookById(long id)
    {
      ResponApi<BookDTO> response = new ResponApi<BookDTO>();
      var book = repository.getById(id);

      if (book == null)
      {
        response.code = 404;
        response.message = "Not Found by Id="+ id;
        response.data = null;
        return NotFound(response);
      }
      var mappedData = mapper.Map<BookDTO>(book);
      response.code = 200;
      response.message = "Success Get By Id";
      response.data = mappedData;

      return Ok(response);
    }

    // PUT: api/TodoItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public ActionResult<ResponApi<Book>> PutBook(long id, [FromForm] BookDTO book)
    {
      ResponApi<Book> response = new ResponApi<Book>();
      var modelDto = repository.getById(id);
      if (modelDto == null)
      {
        response.code = 404;
        response.message = "Not Found by Id="+ id;
        response.data = null;
        return NotFound(response);
      }

      mapper.Map(book, modelDto);
      repository.updateBook(modelDto);
      repository.saveChanges();

      response.code = 200;
      response.message = "Success Get By Id";
      response.data = modelDto;
      return Ok(response);
    }


    // POST: api/TodoItems
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost()]
    public async Task<ActionResult<ResponApi<BookDTO>>> PostTodoItem([FromForm] BookDTO bookDTO)
    {
      ResponApi<BookDTO> response = new ResponApi<BookDTO>();
      var checkDuplicate = repository.checkDuplicate(bookDTO.Name);
      if (checkDuplicate)
      {
        return BadRequest("Data Todo sudah tersedia!");
      }

      TodoItem modelDto = repositoryTodo.getById(bookDTO.TodoId);
      if (modelDto == null)
      {
        response.code = 404;
        response.message = "Not Found by Id="+ bookDTO.TodoId;
        response.data = null;
        return NotFound(response);
      }

      var book = new Book
      {
        Id = bookDTO.Id,
        Name = bookDTO.Name,
        Hal = bookDTO.Hal,
        TodoId = bookDTO.TodoId,
        Todo = modelDto
      };

      repository.createBook(book);
      repository.saveChanges();
      var mappedData = mapper.Map<BookDTO>(book);
      response.code = 200;
      response.message = "Success Create Book";
      response.data = mappedData;
      return Ok(response);
    }

    // DELETE: api/TodoItems/5
    [HttpDelete("{id}")]
    public ActionResult<ResponApi<BookDTO>> DeleteBookById(long id)
    {
      ResponApi<BookDTO> response = new ResponApi<BookDTO>();
      var todoItem = repository.getById(id);
      if (todoItem == null)
      {
        response.code = 404;
        response.message = "Data Not Found by Id="+ id;
        response.data = null;
        return NotFound(response);
      }

      repository.deleteBookById(id);
      repository.saveChanges();

      response.code = 200;
      response.message = "Success Delete by Id="+ id;
      response.data = null;

      return Ok(response);
    }
  }
}