using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models.Dtos;
using TodoApi.Models.Mapping;
using TodoApi.Repositories;

namespace TodoApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TasksController(
    ITaskRepository taskRepository,
    IUserRepository userRepository
) : ControllerBase
{
    private readonly ITaskRepository _taskRepository = taskRepository;
    private readonly IUserRepository _userRepository = userRepository;

    [HttpGet]
    public async Task<IActionResult> GetTasks()
    {
        var userIdClaim = User.FindFirstValue("userId");
        if (userIdClaim == null)
            return Unauthorized("User ID claim is missing.");

        if (!int.TryParse(userIdClaim, out var userId))
            return BadRequest("Invalid User ID claim.");
        var user = await _userRepository.GetUser(userId);
        if (user == null)
            return NotFound("User not found.");
        var tasks = await _taskRepository.GetTasks(userId);
        return Ok(tasks);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(int id)
    {
        var userIdClaim = User.FindFirstValue("userId");
        if (userIdClaim == null)
            return Unauthorized("User ID claim is missing.");

        if (!int.TryParse(userIdClaim, out var userId))
            return BadRequest("Invalid User ID claim.");
        
        var user = await _userRepository.GetUser(userId);
        if (user == null)
            return NotFound("User not found.");
        
        var task = await _taskRepository.GetTask(id);
        if (task == null)
            return NotFound("Task not found.");
        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> AddTask(AddTaskDto newTask)
    {
        var userIdClaim = User.FindFirstValue("userId");
        if (userIdClaim == null)
            return Unauthorized("User ID claim is missing.");
        if (!int.TryParse(userIdClaim, out var userId))
            return BadRequest("Invalid User ID claim.");
        
        var user = await _userRepository.GetUser(userId);
        if (user == null)
            return NotFound("User not found.");
        
        var task = newTask.ToEntity(user);
        await _taskRepository.AddTask(task);
        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTask(EditTaskDto newTask)
    {
        var userIdClaim = User.FindFirstValue("userId");
        if (userIdClaim == null)
            return Unauthorized("User ID claim is missing.");

        if (!int.TryParse(userIdClaim, out var userId))
            return BadRequest("Invalid User ID claim.");
        
        var user = await _userRepository.GetUser(userId);
        if (user == null)
            return NotFound("User not found.");
        var task = await _taskRepository.GetTask(newTask.Id);
        if (task == null)
            return NotFound("Invalid Task Id.");
        newTask.ToEntity(task); 
        await _taskRepository.EditTask(task);
        return Ok(task);
    }
    [HttpDelete]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var userIdClaim = User.FindFirstValue("userId");
        if (userIdClaim == null)
            return Unauthorized("User ID claim is missing.");

        if (!int.TryParse(userIdClaim, out var userId))
            return BadRequest("Invalid User ID claim.");
        
        var user = await _userRepository.GetUser(userId);
        if (user == null)
            return NotFound("User not found.");
        
        var task = await _taskRepository.GetTask(id);
        if (task == null)
            return NotFound("Invalid Task Id.");
        await _taskRepository.DeleteTask(id); 
        return Ok();
    }
}
