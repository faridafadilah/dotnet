using AutoMapper;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Profiles
{

  public class TodoProfile : Profile
  {
    public TodoProfile()
    {
      CreateMap<TodoItem, ResTodoDTO>();
      CreateMap<Book, BookDTO>();
      CreateMap<TodoItem, TodoItemDTO>();
      CreateMap<TodoItemDTO, TodoItem>();
    }
  }
}