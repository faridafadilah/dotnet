using TodoApi.Models;

namespace TodoApi.Data.Interface 
{
  public interface IUserRepository
  {
    User login(string username, string password);
    User register(User user, string password);
    bool saveChanges();
    User getById(long id);
  }
}