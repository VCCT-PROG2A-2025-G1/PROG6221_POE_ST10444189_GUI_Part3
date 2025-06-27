using System;
using System.Collections.Generic;
using System.Windows.Shell;

namespace PROG6221_POE_ST10444189_GUI
{
    public class ChatBot
    {
        //----------------------------------------------\\
        // Track the current topic of conversation
        private string currentTopic = "";
        //----------------------------------------------\\

        //----------------------------------------------\\
        // Variable to store user's name
        private string userName = "";
        //----------------------------------------------\\

        //----------------------------------------------\\
        // Variable to store user's favorite topic
        private string favoriteTopic = "";
        //----------------------------------------------\\

        //----------------------------------------------\\
        // Variable to store random number
        Random random = new Random();
        //----------------------------------------------\\

        //----------------------------------------------\\
        // Variable to store a task
        CyberTask task = new CyberTask();
        //----------------------------------------------\\

        //----------------------------------------------\\
        // Task Manager instance
        private TaskManager taskManager = new TaskManager();
        //----------------------------------------------\\

        //----------------------------------------------\\
        // Activity Log Manager instance
        private ActivityLogManager activityLog = new ActivityLogManager();
        //----------------------------------------------\\

        //----------------------------------------------\\
        // Enhanced NLP instance
        private EnhancedNLP nlpProcessor = new EnhancedNLP();
        //----------------------------------------------\\

        //----------------------------------------------\\
        // Quiz instance
        private CybersecurityQuiz quiz = new CybersecurityQuiz();
        //----------------------------------------------\\

        //----------------------------------------------\\
        // Activity log pagination
        private int currentActivityPage = 0;
        //----------------------------------------------\\

        //----------------------------------------------\\
        // Enum to track conversation steps
        private enum ConversationStep
        {
            None,
            AwaitingTaskReminder
        }
        //----------------------------------------------\\

        //----------------------------------------------\\
        // Variable to track the current step in the conversation
        private ConversationStep currentStep = ConversationStep.None;
        //----------------------------------------------\\

        //----------------------------------------------\\
        // Method to set the user's name
        public void SetUserName(string name)
        {
            userName = name;
        }
        //----------------------------------------------\\

        //----------------------------------------------\\
        // Method to respond based on user input with enhanced NLP
        public string Respond(string userInput)
        {
            //---------------------------------------------------------\\
            // Convert input to lowercase for case-insensitive comparison
            string input = userInput.ToLower().Trim();
            string response = "";

            // Log the conversation
            activityLog.LogConversation(userInput, "Processing...");

            // Perform NLP analysis
            NLPAnalysis analysis = nlpProcessor.AnalyzeInput(userInput);
            //---------------------------------------------------------\\

            //---------------------------------------------------------\\
            // PRIORITY: Handle quiz interactions first
            if (quiz.IsQuizActive())
            {
                // User is in the middle of a quiz, process their answer
                response = quiz.ProcessAnswer(userInput);

                // Check if quiz just ended to log the attempt
                if (!quiz.IsQuizActive() && response.Contains("Quiz Complete"))
                {
                    // Extract score from response for logging
                    LogQuizCompletion(response);
                }

                return response;
            }

            // Check for quiz-related commands using enhanced NLP
            if (analysis.DetectedIntent == "quiz_request" || IsQuizCommand(input))
            {
                response = quiz.StartQuiz();
                activityLog.LogConversationTopic("Quiz", userInput);
                return response;
            }

            if (input == "quiz status" || input == "quiz info")
            {
                return quiz.GetQuizStatus();
            }
            //---------------------------------------------------------\\

            //---------------------------------------------------------\\
            // PRIORITY: Handle task creation flow
            if (currentStep == ConversationStep.AwaitingTaskReminder)
            {
                return HandleTaskReminderResponse(userInput);
            }

            // Handle task management commands BEFORE task creation
            if (IsTaskManagementCommand(input))
            {
                return HandleTaskManagementCommands(input);
            }

            // Enhanced task creation using NLP
            if (analysis.DetectedIntent == "task_creation" || !string.IsNullOrEmpty(analysis.ExtractedTaskName))
            {
                return HandleTaskCreation(analysis);
            }
            //---------------------------------------------------------\\

            //---------------------------------------------------------\\
            // Handle activity log requests
            if (IsActivityLogRequest(input))
            {
                return HandleActivityLogRequest(input);
            }
            //---------------------------------------------------------\\

            //---------------------------------------------------------\\
            // Handle task management commands
            if (IsTaskManagementCommand(input))
            {
                return HandleTaskManagementCommands(input);
            }
            //---------------------------------------------------------\\

            //---------------------------------------------------------\\
            // Ask for user name if not set
            if (string.IsNullOrEmpty(userName) && input.Contains("my name is"))
            {
                userName = ExtractName(userInput);
                response = $"Nice to meet you, {userName}! How can I assist you today?";
                return response;
            }
            //---------------------------------------------------------\\

            //---------------------------------------------------------\\
            // Set favorite topic if mentioned
            if (input.Contains("i like"))
            {
                favoriteTopic = ExtractTopic(userInput);
                response = $"That's awesome, {userName}! I'll remember that you like {favoriteTopic}.";
                activityLog.LogConversationTopic(favoriteTopic, userInput);
                return response;
            }
            //---------------------------------------------------------\\

            //---------------------------------------------------------\\
            // Handle name recall
            if (input.Contains("what's my name") || input.Contains("do you remember my name"))
            {
                response = string.IsNullOrEmpty(userName)
                    ? "Hmm, I don't think I know your name yet. Just say 'My name is ___'."
                    : $"Of course! Your name is {userName}.";
                return response;
            }
            //---------------------------------------------------------\\

            //---------------------------------------------------------\\
            // Handle favorite topic recall
            if (input.Contains("what's my favorite topic"))
            {
                if (string.IsNullOrEmpty(favoriteTopic))
                {
                    response = "I'm not sure what your favorite topic is. Just say 'I like ___'.";
                }
                else
                {
                    response = $"You told me you like {favoriteTopic}. Want to learn more about it?";
                    currentTopic = favoriteTopic;
                }
                return response;
            }
            //---------------------------------------------------------\\

            //---------------------------------------------------------\\
            // Handle "no" responses to follow-up questions
            if ((input == "no" || input.Contains("no thanks") || input.Contains("not really")) && !string.IsNullOrEmpty(currentTopic))
            {
                response = $"No problem, {userName}. If you change your mind, just ask about {currentTopic} again!";
                currentTopic = "";
                return response;
            }
            //---------------------------------------------------------\\

            //---------------------------------------------------------\\
            // Handle follow-up on a current topic
            if ((input.Contains("more") || input.Contains("yes") || input.Contains("tell me more")) && !string.IsNullOrEmpty(currentTopic))
            {
                response = GiveFollowUpTip();
                return response;
            }
            //---------------------------------------------------------\\

            //---------------------------------------------------------\\
            // Enhanced cybersecurity topic detection using NLP
            if (!string.IsNullOrEmpty(analysis.DetectedTopic))
            {
                currentTopic = analysis.DetectedTopic;
                response = HandleCybersecurityTopic(analysis.DetectedTopic, userInput);
                activityLog.LogConversationTopic(analysis.DetectedTopic, userInput);
                return response;
            }
            //---------------------------------------------------------\\

            //---------------------------------------------------------\\
            // Handle specific questions with detailed responses
            if (HandleSpecificQuestions(input, out response))
            {
                return response;
            }
            //---------------------------------------------------------\\

            //---------------------------------------------------------\\
            // Enhanced sentiment detection using NLP
            if (!string.IsNullOrEmpty(analysis.Sentiment))
            {
                response = RespondToSentiment(analysis.Sentiment, userInput);
                if (!string.IsNullOrEmpty(response))
                    return response;
            }
            //---------------------------------------------------------\\

            //---------------------------------------------------------\\
            // Handle general conversation
            response = HandleGeneralConversation(input);
            return response;
            //---------------------------------------------------------\\
        }
        //----------------------------------------------\\

        //---------------------------------------------------------\\
        // Handle task creation using NLP analysis
        private string HandleTaskCreation(NLPAnalysis analysis)
        {
            string taskTitle = analysis.ExtractedTaskName;
            if (string.IsNullOrEmpty(taskTitle))
            {
                // Try to extract from original input if NLP didn't catch it
                taskTitle = ExtractTaskTitle(analysis.OriginalInput);
            }

            if (!string.IsNullOrEmpty(taskTitle))
            {
                task = new CyberTask();
                task.Title = taskTitle;
                task.Description = GenerateCyberDescription(taskTitle);

                currentStep = ConversationStep.AwaitingTaskReminder;
                return $"Task added with the description \"{task.Description}\" Would you like a reminder?";
            }
            else
            {
                return "I'd love to help you create a task! What would you like to be reminded about? You can say something like 'Remind me to update my passwords' or 'Add task - check privacy settings'.";
            }
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Handle task reminder response
        private string HandleTaskReminderResponse(string userInput)
        {
            string input = userInput.ToLower().Trim();
            string response = "";

            if (input.Contains("yes"))
            {
                int days = ExtractDaysFromInput(userInput);
                task.ReminderDate = DateTime.Now.AddDays(days);
                response = $"Got it! I'll remind you in {days} day{(days != 1 ? "s" : "")}.";
            }
            else if (input.Contains("no"))
            {
                task.ReminderDate = null;
                response = "No reminder set.";
            }
            else
            {
                return "Please respond with 'Yes' or 'No' if you'd like a reminder.";
            }

            // Add task to manager and log the activity
            string result = taskManager.AddTask(task.Title, task.Description, task.ReminderDate);
            activityLog.LogTaskCreated(task.Title, task.ReminderDate);

            currentStep = ConversationStep.None;
            return response + " " + result;
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Handle cybersecurity topics with enhanced responses
        private string HandleCybersecurityTopic(string topic, string userInput)
        {
            switch (topic)
            {
                case "password":
                    return CyberTips.passwordTips[random.Next(CyberTips.passwordTips.Count)] +
                           " Would you like to know more about password security?";
                case "phishing":
                    return CyberTips.phishingTips[random.Next(CyberTips.phishingTips.Count)] +
                           " Want to learn more about identifying phishing attempts?";
                case "privacy":
                    return CyberTips.privacyTips[random.Next(CyberTips.privacyTips.Count)] +
                           " Would you like additional privacy protection tips?";
                case "scam":
                    return CyberTips.scammingTips[random.Next(CyberTips.scammingTips.Count)] +
                           " Would you like more tips about avoiding scams?";
                case "malware":
                    return "Malware can seriously damage your devices and steal your data. Keep your antivirus updated, avoid suspicious downloads, and regularly scan your system. Want to learn more about malware protection?";
                case "update":
                    return "Keeping software updated is crucial for security! Updates often fix vulnerabilities that cybercriminals exploit. Enable automatic updates when possible. Want tips on update management?";
                case "backup":
                    return "Regular backups protect you from data loss due to ransomware, hardware failure, or accidents. Follow the 3-2-1 rule: 3 copies, 2 different media, 1 offsite. Need help creating a backup strategy?";
                default:
                    return "That's an important cybersecurity topic! What specific aspect would you like to learn about?";
            }
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Check if input is quiz-related command
        private bool IsQuizCommand(string input)
        {
            return input == "start quiz" || input == "quiz" || input == "take quiz" ||
                   input == "cybersecurity quiz" || input.Contains("test me") ||
                   input.Contains("challenge me");
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Check if input is activity log request
        private bool IsActivityLogRequest(string input)
        {
            return input.Contains("activity log") || input.Contains("show activity") ||
                   input.Contains("my activity") || input.Contains("activity history") ||
                   input.Contains("show more activities") || input.Contains("activity stats") ||
                   input.Contains("what have i done");
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Handle activity log requests
        private string HandleActivityLogRequest(string input)
        {
            if (input.Contains("show more activities"))
            {
                currentActivityPage++;
                return activityLog.FormatActivityLog(currentActivityPage, 10);
            }
            else if (input.Contains("activity stats") || input.Contains("statistics"))
            {
                return activityLog.GetActivityStatistics();
            }
            else
            {
                currentActivityPage = 0;
                return activityLog.FormatActivityLog(currentActivityPage, 10);
            }
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Check if input is task management command
        private bool IsTaskManagementCommand(string input)
        {
            return input.Contains("show tasks") || input.Contains("list tasks") ||
                   input.Contains("my tasks") || input.Contains("view tasks") ||
                   input.Contains("complete task") || input.Contains("delete task") ||
                   input.Contains("task statistics") || input.Contains("pending tasks") ||
                   System.Text.RegularExpressions.Regex.IsMatch(input, @"(complete|delete)\s+task\s+\d+");
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Handle task management commands
        private string HandleTaskManagementCommands(string input)
        {
            if (input.Contains("show tasks") || input.Contains("list tasks") ||
                input.Contains("my tasks") || input.Contains("view tasks"))
            {
                return taskManager.FormatTaskList();
            }
            else if (input.Contains("pending tasks"))
            {
                return taskManager.FormatTaskList(taskManager.GetPendingTasks());
            }
            else if (input.Contains("task statistics"))
            {
                return taskManager.GetTaskStatistics();
            }

            // Handle complete task commands - look for "complete task" followed by a number
            if (input.Contains("complete task"))
            {
                var match = System.Text.RegularExpressions.Regex.Match(input, @"complete\s+task\s+(\d+)");
                if (match.Success)
                {
                    int taskId = int.Parse(match.Groups[1].Value);
                    var task = taskManager.GetTaskById(taskId);
                    string result = taskManager.CompleteTask(taskId);
                    if (task != null && result.Contains("completed"))
                    {
                        activityLog.LogTaskCompleted(task.Title);
                    }
                    return result;
                }
                else
                {
                    return "To complete a task, please specify the task ID. For example: 'Complete task 1'";
                }
            }

            // Handle delete task commands - look for "delete task" followed by a number
            if (input.Contains("delete task"))
            {
                var match = System.Text.RegularExpressions.Regex.Match(input, @"delete\s+task\s+(\d+)");
                if (match.Success)
                {
                    int taskId = int.Parse(match.Groups[1].Value);
                    var task = taskManager.GetTaskById(taskId);
                    string result = taskManager.DeleteTask(taskId);
                    if (task != null && result.Contains("deleted"))
                    {
                        activityLog.LogTaskDeleted(task.Title);
                    }
                    return result;
                }
                else
                {
                    return "To delete a task, please specify the task ID. For example: 'Delete task 1'";
                }
            }

            return "I can help you manage tasks! Try saying 'show my tasks', 'complete task [ID]', or 'delete task [ID]'.";
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Log quiz completion
        private void LogQuizCompletion(string response)
        {
            try
            {
                // Extract score from response using regex
                var scoreMatch = System.Text.RegularExpressions.Regex.Match(response, @"(\d+)/(\d+)\s*\((\d+)%\)");
                if (scoreMatch.Success)
                {
                    int score = int.Parse(scoreMatch.Groups[1].Value);
                    int total = int.Parse(scoreMatch.Groups[2].Value);
                    double percentage = double.Parse(scoreMatch.Groups[3].Value);

                    activityLog.LogQuizAttempt(score, total, percentage);
                }
            }
            catch
            {
                // Fallback logging if parsing fails
                activityLog.LogConversationTopic("Quiz Completed", "User completed cybersecurity quiz");
            }
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Extract task title from various input formats
        private string ExtractTaskTitle(string input)
        {
            string lowerInput = input.ToLower();

            // Multiple patterns for task extraction
            var patterns = new List<string>
            {
                @"add task[:\-\s]+(.+)",
                @"create task[:\-\s]+(.+)",
                @"new task[:\-\s]+(.+)",
                @"remind me to\s+(.+)",
                @"task[:\-\s]+(.+)",
                @"reminder[:\-\s]+(.+)",
                @"i need to\s+(.+)",
                @"help me remember to\s+(.+)",
                @"don't forget to\s+(.+)"
            };

            foreach (var pattern in patterns)
            {
                var match = System.Text.RegularExpressions.Regex.Match(input, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if (match.Success && match.Groups.Count > 1)
                {
                    return match.Groups[1].Value.Trim();
                }
            }

            return "";
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Generate cybersecurity-related description based on task title
        private string GenerateCyberDescription(string title)
        {
            string lowerTitle = title.ToLower();

            if (lowerTitle.Contains("privacy"))
                return "Review account privacy settings to ensure your data is protected.";
            else if (lowerTitle.Contains("password"))
                return "Update and strengthen your passwords for better security.";
            else if (lowerTitle.Contains("backup"))
                return "Create secure backups of your important data.";
            else if (lowerTitle.Contains("software") || lowerTitle.Contains("update"))
                return "Keep your software updated to protect against security vulnerabilities.";
            else if (lowerTitle.Contains("antivirus") || lowerTitle.Contains("security"))
                return "Run security scans to protect your devices from malware.";
            else if (lowerTitle.Contains("email"))
                return "Review email security settings and check for suspicious messages.";
            else if (lowerTitle.Contains("wifi") || lowerTitle.Contains("network"))
                return "Secure your network connections and avoid public WiFi for sensitive activities.";
            else if (lowerTitle.Contains("social media"))
                return "Review and update your social media privacy and security settings.";
            else if (lowerTitle.Contains("browser"))
                return "Update browser settings for enhanced privacy and security.";
            else
                return $"Complete the cybersecurity task: {title}";
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Extract number of days from reminder input
        private int ExtractDaysFromInput(string input)
        {
            string lowerInput = input.ToLower();

            // Look for patterns like "in 3 days", "3 days", "remind me in 3"
            var match = System.Text.RegularExpressions.Regex.Match(lowerInput, @"(\d+)\s*day");
            if (match.Success)
            {
                return int.Parse(match.Groups[1].Value);
            }

            // Look for just numbers
            string[] words = lowerInput.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (int.TryParse(words[i], out int days))
                {
                    return days;
                }
            }

            // Default to 1 day if no number found
            return 1;
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Handle specific detailed questions
        private bool HandleSpecificQuestions(string input, out string response)
        {
            response = "";

            //---------------------------------------------------------\\
            if (input.Contains("tell me about password safety") || input.Contains("password safety"))
            {
                currentTopic = "password";
                response = "Password safety is crucial! Always use strong, unique passwords for each account. Consider using a password manager and enabling two-factor authentication. Want more specific password tips?";
                return true;
            }
            //---------------------------------------------------------\\

            //---------------------------------------------------------\\
            if (input.Contains("what is phishing") || input.Contains("tell me about phishing"))
            {
                currentTopic = "phishing";
                response = "Phishing is when scammers impersonate trusted sources like banks or companies to trick you into giving away personal information. They often use fake emails or websites. Would you like tips on how to identify phishing attempts?";
                return true;
            }
            //---------------------------------------------------------\\

            //---------------------------------------------------------\\
            if (input.Contains("tell me about privacy") || input.Contains("tell me something about privacy"))
            {
                currentTopic = "privacy";
                response = "Online privacy involves protecting your personal information from unauthorized access. This includes being careful about what you share on social media and using secure connections. Would you like specific privacy protection tips?";
                return true;
            }
            //---------------------------------------------------------\\

            //---------------------------------------------------------\\
            if (input.Contains("how can i browse safely") || input.Contains("safe browsing"))
            {
                response = "For safe browsing: always use HTTPS websites (look for the lock icon), avoid clicking suspicious links, keep your browser updated, and be cautious when downloading files. Would you like more browsing safety tips?";
                return true;
            }
            //---------------------------------------------------------\\

            return false;
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Respond based on detected sentiment using NLP
        private string RespondToSentiment(string sentiment, string input)
        {
            switch (sentiment)
            {
                case "positive":
                    return "That's great to hear! Your positive attitude will help you learn cybersecurity more effectively. What topic interests you most?";
                case "negative":
                    return "I understand this can be concerning. I'm here to help you feel more confident about online security. What specific area would you like to learn about first?";
                case "neutral":
                    return ""; // Let other handlers take care of neutral sentiment
                default:
                    return "";
            }
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Handle general conversation
        private string HandleGeneralConversation(string input)
        {
            //---------------------------------------------------------\\
            switch (input)
            {
                case "how are you?":
                case "how are you":
                    return "I'm great, thanks! I'm excited to help you explore cybersecurity.";

                case "what's your purpose?":
                case "what is your purpose":
                    return "My purpose is to educate and assist you with cybersecurity topics to keep you safe online.";

                case "what can i ask you about?":
                case "what can i ask about":
                    return "You can ask me about password safety, phishing scams, safe browsing, privacy protection, and other cybersecurity concerns. You can also type 'start quiz' to test your knowledge, create tasks, or view your activity log!";

                case "help":
                case "commands":
                    return "Here's what I can help you with:\n" +
                           "• Ask about cybersecurity topics (passwords, phishing, privacy, scams)\n" +
                           "• Type 'start quiz' to take a cybersecurity quiz\n" +
                           "• Create tasks: 'Add task - [task name]' or 'Remind me to [task]'\n" +
                           "• Manage tasks: 'show my tasks', 'complete task [ID]', 'delete task [ID]'\n" +
                           "• View activity: 'show activity log' or 'activity stats'\n" +
                           "• Tell me your name: 'My name is [name]'\n" +
                           "• Share interests: 'I like [topic]'";

                case "exit":
                    return "Goodbye! Stay safe online and remember to keep your cybersecurity knowledge up to date.";

                default:
                    return "I didn't quite understand that. You can ask me about cybersecurity topics, take a quiz, manage tasks, or view your activity log. Type 'help' to see all available commands.";
                    //---------------------------------------------------------\\
            }
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Give a follow-up tip
        private string GiveFollowUpTip()
        {
            switch (currentTopic)
            {
                case "password":
                    return GetRandomTip(CyberTips.passwordTips) + " Want another password tip?";
                case "phishing":
                    return GetRandomTip(CyberTips.phishingTips) + " Want another phishing tip?";
                case "privacy":
                    return GetRandomTip(CyberTips.privacyTips) + " Want another privacy tip?";
                case "scam":
                    return GetRandomTip(CyberTips.scammingTips) + " Want another scam prevention tip?";
                default:
                    return "Can you tell me what you'd like to continue talking about?";
            }
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Get a random tip
        private string GetRandomTip(List<string> tips)
        {
            return tips[random.Next(tips.Count)];
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Extract name from input
        private string ExtractName(string input)
        {
            int index = input.ToLower().IndexOf("my name is");
            if (index != -1)
            {
                string name = input.Substring(index + 10).Trim();
                string[] words = name.Split(' ');
                return words.Length > 0 ? words[0] : "Friend";
            }
            return "Friend";
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Extract topic from input
        private string ExtractTopic(string input)
        {
            int index = input.ToLower().IndexOf("i like");
            if (index != -1)
            {
                string topic = input.Substring(index + 6).Trim();
                topic = topic.Replace("the topic", "").Replace("the subject", "").Trim();
                string[] words = topic.Split(' ');
                return words.Length > 0 && !string.IsNullOrWhiteSpace(words[0]) ? words[0] : "cybersecurity";
            }
            return "cybersecurity";
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Get task manager instance for external access
        public TaskManager GetTaskManager()
        {
            return taskManager;
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Get activity log manager instance for external access
        public ActivityLogManager GetActivityLogManager()
        {
            return activityLog;
        }
        //---------------------------------------------------------\\
    }
}

//------------------------oOo End Of File oOo------------------------\\