using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Server.Data;
using ToDoList.Server.Models;
using ToDoList.Server.Models.DTOs;

namespace ToDoList.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public TodoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> CreateTodo([FromBody] CreateTodoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todoList = await _context.TodoLists.FindAsync(dto.TodoListId);
            if (todoList == null)
            {
                return NotFound("Specified TodoList does not exist.");
            }

            var todo = new Todo
            {
                TodoListId = dto.TodoListId,
                Name = dto.Name,
                Description = dto.Description,
                DueDate = dto.DueDate,
                Priority = dto.Priority,
                IsCompleted = false,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoById), new { id = todo.TodoId }, todo);

        }

        [HttpGet("list/{todoListId}")]
        public async Task<ActionResult<IEnumerable<Todo>>> GetAllTodos(int todoListId)
        {
            var todoList = await _context.TodoLists.
                Include(t => t.Todos).
                FirstOrDefaultAsync(t => t.TodoListId == todoListId);


            if (todoList == null)
            {
                return NotFound();
            }

            var result = new
            {
                Name = todoList.Name,
                Todos = todoList.Todos
            };

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodoById(int id)
        {
            var todo = await _context.Todos.FindAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            return todo;
        }

        [HttpGet("date/{date}")]
        public async Task<IActionResult> GetTodosByDate(string date)
        {
            if (!DateTime.TryParse(date, out DateTime selectedDate))
            {
                return BadRequest("Invalid date format.");
            }

            var todos = await _context.Todos
                .Where(t => t.DueDate.Date == selectedDate.Date)
                .ToListAsync();


            return Ok(todos);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Todo>> UpdateTodoById(int id, [FromBody] CreateTodoDto TodoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound("Todo not found");
            }

            todo.Name = TodoDto.Name ?? todo.Name;
            todo.Description = TodoDto.Description ?? todo.Description;

            todo.IsCompleted = TodoDto.IsCompleted;
            todo.UpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(todo);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateTodoIsCompletedStatusByID(int id, [FromBody] UpdateTodoStatusDto dto)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound("Todo not found");
            }

            todo.IsCompleted = dto.IsCompleted;
            todo.UpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return NoContent();


        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoById(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound("Todo not found");
            }

            var todoList = await _context.TodoLists.FindAsync(todo.TodoListId);
            if (todoList != null)
            {
                todoList.UpdatedDate = DateTime.Now;
            }

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return NoContent();

        }

        [HttpGet("upcoming-todos")]
        public async Task<IActionResult> GetUpcomingTodos(int days = 2)
        {

            DateTime upperLimit = DateTime.Now.AddDays(days);

            var upcomingTasks = await _context.Todos
                .Where(t => !t.IsCompleted && t.DueDate >= DateTime.Now && t.DueDate <= upperLimit)
                .OrderBy(t => t.DueDate)
                .ToListAsync();

            return Ok(upcomingTasks);
        }

    }

}
