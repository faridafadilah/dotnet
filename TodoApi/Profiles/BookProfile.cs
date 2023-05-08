using AutoMapper;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Profiles
{

  public class BookProfile : Profile
  {
    public BookProfile()
    {
      CreateMap<Book, BookDTO>();
      CreateMap<BookDTO, Book>();
    }
  }
}