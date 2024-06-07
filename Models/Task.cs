using System;

namespace Dashboard.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DueDate { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
    public class TaskRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DueDate { get; set; }
        public int ProjectId { get; set; }
    }
}
