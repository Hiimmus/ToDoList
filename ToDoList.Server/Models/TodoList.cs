using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ToDoList.Server.Models
{
    public class TodoList
    {
        public int TodoListId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        [JsonIgnore]
        public ICollection<Todo> Todos { get; set; } = new List<Todo>();
    }
}
