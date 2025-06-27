using System;
using System.Collections.Generic;
using System.Linq;

namespace PROG6221_POE_ST10444189_GUI
{
    //----------------------------------------------\\
    // Enhanced CyberTask class with additional properties
    public class CyberTask
    {
        //---------------------------------------------------------\\
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        public CyberTask()
        {
            CreatedDate = DateTime.Now;
            IsCompleted = false;
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Override ToString for better display
        public override string ToString()
        {
            string status = IsCompleted ? "✅ Completed" : "⏳ Pending";
            string reminder = ReminderDate.HasValue ? $" | Reminder: {ReminderDate.Value:dd/MM/yyyy HH:mm}" : "";
            return $"{Title} - {status}{reminder}";
        }
        //---------------------------------------------------------\\
    }
    //----------------------------------------------\\

    //----------------------------------------------\\
    // Task Manager class to handle all task operations
    public class TaskManager
    {
        //---------------------------------------------------------\\
        private List<CyberTask> tasks;
        private int nextId;
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        public TaskManager()
        {
            tasks = new List<CyberTask>();
            nextId = 1;
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Add a new task
        public string AddTask(string title, string description, DateTime? reminderDate = null)
        {
            var task = new CyberTask
            {
                Id = nextId++,
                Title = title,
                Description = description,
                ReminderDate = reminderDate
            };

            tasks.Add(task);
            return $"Task '{title}' added successfully with ID {task.Id}.";
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Get all tasks
        public List<CyberTask> GetAllTasks()
        {
            return tasks.OrderBy(t => t.IsCompleted).ThenBy(t => t.ReminderDate).ToList();
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Get pending tasks
        public List<CyberTask> GetPendingTasks()
        {
            return tasks.Where(t => !t.IsCompleted).OrderBy(t => t.ReminderDate).ToList();
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Mark task as complete
        public string CompleteTask(int taskId)
        {
            var task = tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                return $"Task with ID {taskId} not found.";

            if (task.IsCompleted)
                return $"Task '{task.Title}' is already completed.";

            task.IsCompleted = true;
            task.CompletedDate = DateTime.Now;
            return $"Task '{task.Title}' marked as completed!";
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Delete task
        public string DeleteTask(int taskId)
        {
            var task = tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                return $"Task with ID {taskId} not found.";

            string taskTitle = task.Title;
            tasks.Remove(task);
            return $"Task '{taskTitle}' deleted successfully.";
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Get tasks with upcoming reminders
        public List<CyberTask> GetUpcomingReminders(int hours = 24)
        {
            var cutoffTime = DateTime.Now.AddHours(hours);
            return tasks.Where(t => !t.IsCompleted &&
                               t.ReminderDate.HasValue &&
                               t.ReminderDate.Value <= cutoffTime &&
                               t.ReminderDate.Value >= DateTime.Now)
                       .OrderBy(t => t.ReminderDate)
                       .ToList();
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Get task by ID
        public CyberTask GetTaskById(int taskId)
        {
            return tasks.FirstOrDefault(t => t.Id == taskId);
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Get task statistics
        public string GetTaskStatistics()
        {
            int total = tasks.Count;
            int completed = tasks.Count(t => t.IsCompleted);
            int pending = total - completed;
            int withReminders = tasks.Count(t => t.ReminderDate.HasValue);

            return $"📊 Task Statistics:\n" +
                   $"Total Tasks: {total}\n" +
                   $"Completed: {completed}\n" +
                   $"Pending: {pending}\n" +
                   $"With Reminders: {withReminders}";
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Format task list for display
        public string FormatTaskList(List<CyberTask> taskList = null)
        {
            var displayTasks = taskList ?? GetAllTasks();

            if (!displayTasks.Any())
                return "No tasks found.";

            string result = "📋 Your Tasks:\n\n";
            foreach (var task in displayTasks)
            {
                string status = task.IsCompleted ? "✅" : "⏳";
                string reminder = task.ReminderDate.HasValue ?
                    $"\n   📅 Reminder: {task.ReminderDate.Value:dd/MM/yyyy HH:mm}" : "";

                result += $"{status} [{task.Id}] {task.Title}\n";
                result += $"   📝 {task.Description}{reminder}\n\n";
            }

            return result;
        }
        //---------------------------------------------------------\\
    }
    //----------------------------------------------\\
}

//------------------------oOo End Of File oOo------------------------\\