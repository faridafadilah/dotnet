using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data
{

  public class TodoContext : DbContext
  {
    public TodoContext(DbContextOptions<TodoContext> options) : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<User> Users { get; set; }
  }
}