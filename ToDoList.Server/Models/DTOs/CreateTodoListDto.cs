using System.ComponentModel.DataAnnotations;

namespace ToDoList.Server.Models.DTOs
{
    public class CreateTodoListDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
