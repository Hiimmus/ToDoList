using Microsoft.EntityFrameworkCore;
using ToDoList.Server.Models;

namespace ToDoList.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Todo> Todos { get; set; }
        public DbSet<TodoList> TodoLists { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoList>()
                .HasMany(tl => tl.Todos)
                .WithOne(t => t.TodoList)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }

}