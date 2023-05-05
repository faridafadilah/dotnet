using AutoMapper;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Profiles
{
  public class UserProfile : Profile
  {
    public UserProfile()
    {
      CreateMap<RegisterDto, User>();
    }
  }
}