using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dashboard.Models;
using Dashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _taskService;

        public TasksController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Dashboard.Models.Task>>> GetAllTasks()
        {
            return await _taskService.GetAllTasksAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Dashboard.Models.Task>> GetTaskById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<List<Dashboard.Models.Task>>> GetTasksByProjectId(int projectId)
        {
            var tasks = await _taskService.GetTasksByProjectIdAsync(projectId);

            if (tasks == null || tasks.Count == 0)
            {
                return NotFound();
            }

            return tasks;
        }

        [HttpPost]
        public async Task<ActionResult<Dashboard.Models.Task>> AddTask(TaskRequest taskRequest)
        {
            try
            {
                var newTask = await _taskService.AddTaskAsync(taskRequest);
                return CreatedAtAction(nameof(GetTaskById), new { id = newTask.Id }, newTask);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, Dashboard.Models.Task task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            try
            {
                await _taskService.UpdateTaskAsync(task);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var result = await _taskService.DeleteTaskAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
