using System.ComponentModel.DataAnnotations;

namespace ToDoList.Server.Models.DTOs
{
    public class CreateTodoDto
    {
        [Required]
        public int TodoListId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime DueDate { get; set; }

        public int Priority { get; set; }
    }
}
