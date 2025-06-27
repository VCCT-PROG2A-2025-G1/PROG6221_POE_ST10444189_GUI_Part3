// References
// https://chatgpt.com/
// https://www.w3schools.com/cpp/

using System;
using System.Collections.Generic;
using System.Linq;

namespace PROG6221_POE_ST10444189_GUI
{
    //----------------------------------------------\\
    // Activity Log Entry class to track user actions
    public class ActivityLogEntry
    {
        //---------------------------------------------------------\\
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string ActivityType { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        public ActivityLogEntry()
        {
            Timestamp = DateTime.Now;
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        public override string ToString()
        {
            return $"[{Timestamp:HH:mm:ss}] {ActivityType}: {Description}";
        }
        //---------------------------------------------------------\\
    }
    //----------------------------------------------\\

    //----------------------------------------------\\
    // Activity Log Manager class to handle all activity tracking
    public class ActivityLogManager
    {
        //---------------------------------------------------------\\
        private List<ActivityLogEntry> activities;
        private int nextId;
        private int itemsPerPage;
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        public ActivityLogManager(int itemsPerPage = 10)
        {
            activities = new List<ActivityLogEntry>();
            nextId = 1;
            this.itemsPerPage = itemsPerPage;
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Log a task creation activity
        public void LogTaskCreated(string taskTitle, DateTime? reminderDate = null)
        {
            string reminderInfo = reminderDate.HasValue ?
                $" with reminder on {reminderDate.Value:dd/MM/yyyy HH:mm}" : " without reminder";

            var entry = new ActivityLogEntry
            {
                Id = nextId++,
                ActivityType = "Task Created",
                Description = $"Created task: '{taskTitle}'",
                Details = $"Task '{taskTitle}' was created{reminderInfo}"
            };

            activities.Add(entry);
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Log a task completion activity
        public void LogTaskCompleted(string taskTitle)
        {
            var entry = new ActivityLogEntry
            {
                Id = nextId++,
                ActivityType = "Task Completed",
                Description = $"Completed task: '{taskTitle}'",
                Details = $"Task '{taskTitle}' was marked as completed"
            };

            activities.Add(entry);
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Log a task deletion activity
        public void LogTaskDeleted(string taskTitle)
        {
            var entry = new ActivityLogEntry
            {
                Id = nextId++,
                ActivityType = "Task Deleted",
                Description = $"Deleted task: '{taskTitle}'",
                Details = $"Task '{taskTitle}' was permanently deleted"
            };

            activities.Add(entry);
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Log a quiz attempt activity
        public void LogQuizAttempt(int score, int totalQuestions, double percentage)
        {
            var entry = new ActivityLogEntry
            {
                Id = nextId++,
                ActivityType = "Quiz Completed",
                Description = $"Quiz completed with score: {score}/{totalQuestions} ({percentage:F0}%)",
                Details = $"Cybersecurity quiz completed with {score} correct answers out of {totalQuestions} questions ({percentage:F1}% accuracy)"
            };

            activities.Add(entry);
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Log conversation topic or keyword identification
        public void LogConversationTopic(string topic, string userInput)
        {
            var entry = new ActivityLogEntry
            {
                Id = nextId++,
                ActivityType = "Topic Identified",
                Description = $"Discussed topic: {topic}",
                Details = $"User input: '{userInput}' | Identified topic: {topic}"
            };

            activities.Add(entry);
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Log general conversation interactions
        public void LogConversation(string userInput, string botResponse)
        {
            var entry = new ActivityLogEntry
            {
                Id = nextId++,
                ActivityType = "Conversation",
                Description = $"User: {(userInput.Length > 30 ? userInput.Substring(0, 30) + "..." : userInput)}",
                Details = $"User: {userInput} | Bot: {(botResponse.Length > 100 ? botResponse.Substring(0, 100) + "..." : botResponse)}"
            };

            activities.Add(entry);
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Get recent activities (last n items)
        public List<ActivityLogEntry> GetRecentActivities(int count = 10)
        {
            return activities.OrderByDescending(a => a.Timestamp)
                           .Take(count)
                           .ToList();
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Get activities with pagination
        public List<ActivityLogEntry> GetActivitiesPage(int pageNumber, int pageSize = 10)
        {
            return activities.OrderByDescending(a => a.Timestamp)
                           .Skip(pageNumber * pageSize)
                           .Take(pageSize)
                           .ToList();
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Get total count of activities
        public int GetTotalActivityCount()
        {
            return activities.Count;
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Get activities by type
        public List<ActivityLogEntry> GetActivitiesByType(string activityType)
        {
            return activities.Where(a => a.ActivityType.Equals(activityType, StringComparison.OrdinalIgnoreCase))
                           .OrderByDescending(a => a.Timestamp)
                           .ToList();
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Format activity log for display
        public string FormatActivityLog(int pageNumber = 0, int pageSize = 10)
        {
            var pageActivities = GetActivitiesPage(pageNumber, pageSize);

            if (!pageActivities.Any())
                return "No activities found.";

            string result = "📋 Activity Log:\n\n";

            foreach (var activity in pageActivities)
            {
                result += $"🕒 {activity.Timestamp:dd/MM/yyyy HH:mm:ss}\n";
                result += $"📝 {activity.ActivityType}: {activity.Description}\n\n";
            }

            int totalPages = (int)Math.Ceiling((double)GetTotalActivityCount() / pageSize);
            if (totalPages > 1)
            {
                result += $"Page {pageNumber + 1} of {totalPages}\n";
                if (pageNumber < totalPages - 1)
                {
                    result += "Type 'show more activities' to see more entries.";
                }
            }

            return result;
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Get activity statistics
        public string GetActivityStatistics()
        {
            int totalActivities = activities.Count;
            int tasksCreated = activities.Count(a => a.ActivityType == "Task Created");
            int tasksCompleted = activities.Count(a => a.ActivityType == "Task Completed");
            int quizAttempts = activities.Count(a => a.ActivityType == "Quiz Completed");
            int conversations = activities.Count(a => a.ActivityType == "Conversation");

            return $"📊 Activity Statistics:\n" +
                   $"Total Activities: {totalActivities}\n" +
                   $"Tasks Created: {tasksCreated}\n" +
                   $"Tasks Completed: {tasksCompleted}\n" +
                   $"Quiz Attempts: {quizAttempts}\n" +
                   $"Conversations: {conversations}";
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Clear old activities (keep only recent n items)
        public void CleanupOldActivities(int keepCount = 100)
        {
            if (activities.Count > keepCount)
            {
                activities = activities.OrderByDescending(a => a.Timestamp)
                                     .Take(keepCount)
                                     .ToList();
            }
        }
        //---------------------------------------------------------\\
    }
    //----------------------------------------------\\
}

//------------------------oOo End Of File oOo------------------------\\