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
        //List to store task lists
        private List<CyberTask> taskList = new List<CyberTask>();
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
        // Method to respond based on user input
        public string Respond(string userInput)
        {
            //---------------------------------------------------------\\
            // Convert input to lowercase for case-insensitive comparison
            string input = userInput.ToLower().Trim();
            string response = "";
            //---------------------------------------------------------\\

            //---------------------------------------------------------\\
            // PRIORITY: Handle task creation flow first
            if (currentStep == ConversationStep.AwaitingTaskReminder)
            {
                if (input.Contains("yes"))
                {
                    // Extract number of days from user input
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

                task.IsCompleted = false;
                taskList.Add(task);
                currentStep = ConversationStep.None;
                return response;
            }

            // Start task creation - look for "Add task - [task title]" format
            if (input.StartsWith("add task -") || input.StartsWith("add task-"))
            {
                string taskTitle = ExtractTaskTitle(userInput);
                if (!string.IsNullOrEmpty(taskTitle))
                {
                    task = new CyberTask(); // Reset temp task
                    task.Title = taskTitle;

                    // Generate cybersecurity-related description based on title
                    task.Description = GenerateCyberDescription(taskTitle);

                    currentStep = ConversationStep.AwaitingTaskReminder;
                    return $"Task added with the description \"{task.Description}\" Would you like a reminder?";
                }
                else
                {
                    return "Please provide a task title. Format: 'Add task - [your task title]'";
                }
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
            // PRIORITY: Check for specific cybersecurity topics first (before sentiment detection)
            if (IsPasswordRelated(input))
            {
                currentTopic = "password";
                response = CyberTips.passwordTips[random.Next(CyberTips.passwordTips.Count)] +
                           " Would you like to know more about password security?";
                return response;
            }
            else if (IsScamRelated(input))
            {
                currentTopic = "scam";
                response = CyberTips.scammingTips[random.Next(CyberTips.scammingTips.Count)] +
                           " Would you like more tips about avoiding scams?";
                return response;
            }
            else if (IsPhishingRelated(input))
            {
                currentTopic = "phishing";
                response = CyberTips.phishingTips[random.Next(CyberTips.phishingTips.Count)] +
                           " Want to learn more about identifying phishing attempts?";
                return response;
            }
            else if (IsPrivacyRelated(input))
            {
                currentTopic = "privacy";
                response = CyberTips.privacyTips[random.Next(CyberTips.privacyTips.Count)] +
                           " Would you like additional privacy protection tips?";
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
            // Sentiment Detection (only after topic-specific handling)
            string detectedSentiment = DetectSentiment(input);
            if (!string.IsNullOrEmpty(detectedSentiment))
            {
                response = RespondToSentiment(detectedSentiment, input);
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
        // Extract task title from "Add task - [title]" format
        private string ExtractTaskTitle(string input)
        {
            string lowerInput = input.ToLower();
            int dashIndex = lowerInput.IndexOf(" - ");
            if (dashIndex == -1)
                dashIndex = lowerInput.IndexOf("- ");
            if (dashIndex == -1)
                dashIndex = lowerInput.IndexOf("-");

            if (dashIndex != -1 && dashIndex + 1 < input.Length)
            {
                return input.Substring(dashIndex + (input[dashIndex + 1] == ' ' ? 2 : 1)).Trim();
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
        // Enhanced keyword detection methods
        private bool IsPasswordRelated(string input)
        {
            return input.Contains("password") || input.Contains("passphrase") ||
                   input.Contains("login") || input.Contains("authentication") ||
                   input.Contains("password tip") || input.Contains("password safety") ||
                   input.Contains("secure password");
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        private bool IsScamRelated(string input)
        {
            return input.Contains("scam") || input.Contains("fraud") ||
                   input.Contains("suspicious call") || input.Contains("phone scam") ||
                   input.Contains("avoid scam");
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        private bool IsPhishingRelated(string input)
        {
            return input.Contains("phishing") || input.Contains("suspicious email") ||
                   input.Contains("fake email") || input.Contains("email scam") ||
                   input.Contains("what is phishing");
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        private bool IsPrivacyRelated(string input)
        {
            return input.Contains("privacy") || input.Contains("personal information") ||
                   input.Contains("data protection") || input.Contains("online privacy") ||
                   input.Contains("social media privacy") || input.Contains("vpn");
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
        // Detect sentiment in user input (refined to avoid conflicts)
        private string DetectSentiment(string input)
        {
            // Only detect sentiment if it's not already covered by topic-specific responses
            if (input.Contains("worried") || input.Contains("scared") || input.Contains("afraid") ||
                input.Contains("anxious") || input.Contains("concerned"))
                return "worried";
            else if (input.Contains("frustrated") || input.Contains("annoyed") || input.Contains("confused") ||
                     input.Contains("don't understand") || input.Contains("help me"))
                return "frustrated";
            else if (input.Contains("happy") || input.Contains("excited") || input.Contains("great") ||
                     input.Contains("awesome") || input.Contains("love"))
                return "happy";
            return "";
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Respond based on detected sentiment
        private string RespondToSentiment(string sentiment, string input)
        {
            switch (sentiment)
            {
                case "worried":
                    return "It's completely understandable to feel worried about online security. I'm here to help you feel more confident. What specific area would you like to learn about first - passwords, phishing, privacy, or scams?";
                case "frustrated":
                    return "I understand this can be overwhelming. Let's take it step by step. What's the main thing you'd like help with?";
                case "happy":
                    return "That's great to hear! Your positive attitude will help you learn cybersecurity more effectively. What topic interests you most?";
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
                    return "You can ask me about password safety, phishing scams, safe browsing, privacy protection, and other cybersecurity concerns.";

                case "exit":
                    return "Goodbye! Stay safe online.";

                default:
                    return "I didn't quite understand that. You can ask me about passwords, phishing, privacy, or scams. Try asking something like 'Tell me about password safety' or 'What is phishing?'";
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
    }
}

//------------------------oOo End Of File oOo------------------------\\