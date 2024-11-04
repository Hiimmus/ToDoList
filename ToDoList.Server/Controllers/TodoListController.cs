using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ToDoList.Server.Data;
using ToDoList.Server.Models;
using ToDoList.Server.Models.DTOs;

namespace ToDoList.Server.Controllers
{
    [Route("api/[controller]")]
    public class TodoListController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public TodoListController(ApplicationDbContext context)
        {

            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<TodoList>> CreateTodoList([FromBody] CreateTodoListDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todoList = new TodoList
            {
                Name = dto.Name,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };
            _context.TodoLists.Add(todoList);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoListById), new { id = todoList.TodoListId }, todoList);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoList>>> GetTodoLists()
        {

            var todoLists = await _context.TodoLists.ToListAsync();
            if (todoLists == null)
            {
                return NotFound();
            }

            return Ok(todoLists);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoList>> GetTodoListById(int id)
        {
            var todoList = await _context.TodoLists.FindAsync(id);

            if (todoList == null)
            {
                return NotFound();
            }

            return todoList;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoList>> DeleteTodoListById(int id)
        {
            var todoList = await _context.TodoLists.FindAsync(id);
            if (todoList == null)
            {
                return NotFound("Todo List not found");
            }

            _context.TodoLists.Remove(todoList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }

}
