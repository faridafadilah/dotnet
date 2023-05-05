using TodoApi.Models;
using TodoApi.Data.Interface;

namespace TodoApi.Data.Implementations
{
  public class UserRepository : IUserRepository
  {
    private readonly TodoContext context;

    public UserRepository(TodoContext context)
    {
      this.context = context;
    }

    public User register(User user, string password)
    {
      if (string.IsNullOrWhiteSpace(password))
      {
        throw new AppException("Password is required");
      }

      if (context.Users.Any(x => x.username == user.username))
      {
        throw new AppException("Username \"" + user.username + "\" is already taken");
      }
      byte[] passwordHash, passwordSalt;
      CreatePasswordHash(password, out passwordHash, out passwordSalt);
      user.passwordHash = passwordHash;
      user.passwordSalt = passwordSalt;
      context.Users.Add(user);
      context.SaveChanges();
      return user;
    }
    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
      if (password == null) throw new ArgumentNullException("password");
      if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

      using (var hmac = new System.Security.Cryptography.HMACSHA512())
      {
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
      }
    }

    public User login(string username, string password)
    {
      if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
      {
        return null;
      }
      var user = context.Users.SingleOrDefault(x => x.username == username);
      if (user == null)
      {
        return null;
      }
      if (!VerifyPasswordHash(password, user.passwordHash, user.passwordSalt))
      {
        return null;
      }
      return user;
    }

    private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
      if (password == null) throw new ArgumentNullException("password");
      if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
      if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
      if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");
      using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
      {
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        for (int i = 0; i < computedHash.Length; i++)
        {
          if (computedHash[i] != storedHash[i]) return false;
        }
      }
      return true;
    }

    public bool saveChanges()
    {
      return (context.SaveChanges() >= 0);
    }

    public User getById(long id)
    {
      return context.Users.FirstOrDefault(x => x.id == id);

    }
  }
}