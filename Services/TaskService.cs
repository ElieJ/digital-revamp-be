using System.Collections.Generic;
using System.Threading.Tasks;
using Dashboard.Data;
using Dashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Services
{
    public class TaskService
    {
        private readonly ApplicationDbContext _context;
        private readonly ProjectService _projectService;

        public TaskService(ApplicationDbContext context, ProjectService projectService)
        {
            _context = context;
            _projectService = projectService;
        }

        public async Task<List<Dashboard.Models.Task>> GetAllTasksAsync()
        {
            return await _context.Tasks.Include(t => t.Project).ToListAsync();
        }

        public async Task<Dashboard.Models.Task> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks.Include(t => t.Project).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Dashboard.Models.Task>> GetTasksByProjectIdAsync(int projectId)
        {
            return await _context.Tasks
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.Project)
                .ToListAsync();
        }

        public async Task<Dashboard.Models.Task> AddTaskAsync(TaskRequest taskRequest)
        {
            var project = await _projectService.GetProjectByIdAsync(taskRequest.ProjectId);
            if (project == null)
            {
                throw new ArgumentException("Invalid ProjectId");
            }

            var task = new Dashboard.Models.Task
            {
                Title = taskRequest.Title,
                Description = taskRequest.Description,
                DueDate = taskRequest.DueDate.ToString(),
                ProjectId = taskRequest.ProjectId,
                Project = project
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<Dashboard.Models.Task> UpdateTaskAsync(Dashboard.Models.Task task)
        {
            var project = await _projectService.GetProjectByIdAsync(task.ProjectId);
            if (project == null)
            {
                throw new ArgumentException("Invalid ProjectId");
            }
            task.Project = project;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return false;
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
