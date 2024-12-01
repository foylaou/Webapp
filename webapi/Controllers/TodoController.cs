using webapi.Models;

namespace webapi.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private static List<Todo> _todos = new List<Todo>();
    private static int _idCounter = 1;

    [HttpGet]
    public ActionResult<IEnumerable<Todo>> GetAll()
    {
        return Ok(_todos);
    }

    [HttpGet("{id}")]
    public ActionResult<Todo> GetById(int id)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
            return NotFound();
        
        return Ok(todo);
    }

    [HttpPost]
    public ActionResult<Todo> Create(Todo todo)
    {
        todo.Id = _idCounter++;
        _todos.Add(todo);
        return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Todo todo)
    {
        var existingTodo = _todos.FirstOrDefault(t => t.Id == id);
        if (existingTodo == null)
            return NotFound();

        existingTodo.Title = todo.Title;
        existingTodo.IsCompleted = todo.IsCompleted;
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
            return NotFound();

        _todos.Remove(todo);
        return NoContent();
    }


}