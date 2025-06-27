// References
// https://chatgpt.com/
// https://www.w3schools.com/cpp/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PROG6221_POE_ST10444189_GUI
{
    //----------------------------------------------\\
    // Enhanced NLP class for advanced keyword and phrase detection
    public class EnhancedNLP
    {
        //---------------------------------------------------------\\
        private Dictionary<string, List<string>> topicKeywords;
        private Dictionary<string, List<string>> phrasePatterns;
        private Dictionary<string, List<string>> actionKeywords;
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        public EnhancedNLP()
        {
            InitializeKeywords();
            InitializePhrasePatterns();
            InitializeActionKeywords();
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Initialize comprehensive keyword dictionary
        private void InitializeKeywords()
        {
            topicKeywords = new Dictionary<string, List<string>>
            {
                ["password"] = new List<string>
                {
                    "password", "passphrase", "login", "authentication", "credential", "pin", "passcode",
                    "secure password", "strong password", "password strength", "password manager",
                    "password security", "password policy", "password reset", "forgotten password",
                    "password breach", "password leak", "password reuse", "unique password"
                },

                ["phishing"] = new List<string>
                {
                    "phishing", "phish", "fake email", "suspicious email", "email scam", "spoofed email",
                    "fraudulent email", "malicious link", "suspicious link", "email fraud",
                    "identity theft", "email impersonation", "fake website", "spoofed website",
                    "suspicious attachment", "email verification", "account verification"
                },

                ["privacy"] = new List<string>
                {
                    "privacy", "personal information", "data protection", "personal data", "private data",
                    "data privacy", "online privacy", "digital privacy", "information sharing",
                    "data collection", "tracking", "surveillance", "vpn", "virtual private network",
                    "encryption", "anonymity", "data breach", "privacy settings", "social media privacy"
                },

                ["scam"] = new List<string>
                {
                    "scam", "fraud", "fraudulent", "suspicious call", "phone scam", "robocall",
                    "tech support scam", "romance scam", "investment scam", "lottery scam",
                    "advance fee fraud", "fake charity", "identity fraud", "credit card fraud",
                    "social engineering", "manipulation", "deception", "fake offer"
                },

                ["malware"] = new List<string>
                {
                    "malware", "virus", "trojan", "ransomware", "spyware", "adware", "rootkit",
                    "keylogger", "worm", "botnet", "malicious software", "infected file",
                    "antivirus", "anti-malware", "virus scan", "quarantine", "malware removal"
                },

                ["update"] = new List<string>
                {
                    "update", "software update", "security update", "patch", "security patch",
                    "system update", "firmware update", "browser update", "app update",
                    "outdated software", "vulnerable software", "latest version"
                },

                ["backup"] = new List<string>
                {
                    "backup", "data backup", "backup strategy", "cloud backup", "file backup",
                    "system backup", "restore", "data recovery", "backup copy", "sync",
                    "external drive", "backup schedule", "automated backup"
                }
            };
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Initialize phrase patterns for better context understanding
        private void InitializePhrasePatterns()
        {
            phrasePatterns = new Dictionary<string, List<string>>
            {
                ["task_creation"] = new List<string>
                {
                    "add task", "create task", "new task", "make task", "set task", "task for",
                    "remind me to", "i need to", "help me remember", "schedule task",
                    "add reminder", "create reminder", "set reminder", "reminder for",
                    "don't forget to", "remember to", "task reminder"
                },

                ["quiz_request"] = new List<string>
                {
                    "start quiz", "take quiz", "quiz me", "test me", "cybersecurity quiz",
                    "security quiz", "test my knowledge", "quiz time", "challenge me",
                    "practice quiz", "knowledge test", "security test"
                },

                ["help_request"] = new List<string>
                {
                    "help me", "how do i", "what should i do", "can you help", "i need help",
                    "assist me", "guide me", "show me how", "explain how", "tell me how",
                    "what is", "how to", "help with", "confused about", "don't understand"
                },

                ["information_request"] = new List<string>
                {
                    "tell me about", "what is", "explain", "information about", "learn about",
                    "more about", "details about", "describe", "define", "meaning of",
                    "how does", "why is", "when should", "where can"
                },

                ["concern_expression"] = new List<string>
                {
                    "worried about", "concerned about", "afraid of", "scared of", "anxious about",
                    "nervous about", "unsure about", "confused about", "suspicious of",
                    "think i've been", "might have been", "could be"
                }
            };
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Initialize action keywords
        private void InitializeActionKeywords()
        {
            actionKeywords = new Dictionary<string, List<string>>
            {
                ["view"] = new List<string>
                {
                    "show", "display", "view", "list", "see", "check", "look at", "get"
                },

                ["create"] = new List<string>
                {
                    "add", "create", "make", "new", "set up", "establish", "build", "generate"
                },

                ["delete"] = new List<string>
                {
                    "delete", "remove", "clear", "erase", "cancel", "get rid of", "eliminate"
                },

                ["complete"] = new List<string>
                {
                    "complete", "finish", "done", "mark as done", "mark complete", "finished"
                },

                ["learn"] = new List<string>
                {
                    "learn", "teach me", "educate", "explain", "show me", "tell me"
                }
            };
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Enhanced keyword detection in full sentences
        public string DetectTopic(string input)
        {
            string lowerInput = input.ToLower();
            var detectedTopics = new Dictionary<string, int>();

            // Score each topic based on keyword matches
            foreach (var topicGroup in topicKeywords)
            {
                int score = 0;
                foreach (var keyword in topicGroup.Value)
                {
                    if (lowerInput.Contains(keyword))
                    {
                        // Give higher score for exact matches
                        if (lowerInput.Contains(" " + keyword + " ") ||
                            lowerInput.StartsWith(keyword + " ") ||
                            lowerInput.EndsWith(" " + keyword))
                        {
                            score += 3;
                        }
                        else
                        {
                            score += 1;
                        }
                    }
                }

                if (score > 0)
                {
                    detectedTopics[topicGroup.Key] = score;
                }
            }

            // Return the topic with the highest score
            if (detectedTopics.Any())
            {
                return detectedTopics.OrderByDescending(t => t.Value).First().Key;
            }

            return "";
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Detect user intent from phrases
        public string DetectIntent(string input)
        {
            string lowerInput = input.ToLower();

            foreach (var intentGroup in phrasePatterns)
            {
                foreach (var pattern in intentGroup.Value)
                {
                    if (lowerInput.Contains(pattern))
                    {
                        return intentGroup.Key;
                    }
                }
            }

            return "";
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Detect action keywords
        public string DetectAction(string input)
        {
            string lowerInput = input.ToLower();

            foreach (var actionGroup in actionKeywords)
            {
                foreach (var action in actionGroup.Value)
                {
                    if (lowerInput.Contains(action))
                    {
                        return actionGroup.Key;
                    }
                }
            }

            return "";
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Extract entities (like task names) from input
        public string ExtractTaskName(string input)
        {
            string lowerInput = input.ToLower();

            // Patterns for task extraction
            var patterns = new List<string>
            {
                @"add task[:\-\s]+(.+)",
                @"create task[:\-\s]+(.+)",
                @"new task[:\-\s]+(.+)",
                @"remind me to\s+(.+)",
                @"task[:\-\s]+(.+)",
                @"reminder[:\-\s]+(.+)"
            };

            foreach (var pattern in patterns)
            {
                var match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
                if (match.Success && match.Groups.Count > 1)
                {
                    return match.Groups[1].Value.Trim();
                }
            }

            return "";
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Analyze sentiment of the input
        public string AnalyzeSentiment(string input)
        {
            string lowerInput = input.ToLower();

            var positiveWords = new List<string>
            {
                "good", "great", "excellent", "awesome", "fantastic", "wonderful",
                "amazing", "perfect", "love", "like", "happy", "excited", "thank"
            };

            var negativeWords = new List<string>
            {
                "bad", "terrible", "awful", "horrible", "hate", "dislike", "angry",
                "frustrated", "confused", "worried", "scared", "concerned", "problem"
            };

            var neutralWords = new List<string>
            {
                "okay", "fine", "normal", "average", "standard", "regular"
            };

            int positiveScore = positiveWords.Count(word => lowerInput.Contains(word));
            int negativeScore = negativeWords.Count(word => lowerInput.Contains(word));
            int neutralScore = neutralWords.Count(word => lowerInput.Contains(word));

            if (positiveScore > negativeScore && positiveScore > neutralScore)
                return "positive";
            else if (negativeScore > positiveScore && negativeScore > neutralScore)
                return "negative";
            else if (neutralScore > 0)
                return "neutral";

            return "";
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Get comprehensive analysis of user input
        public NLPAnalysis AnalyzeInput(string input)
        {
            return new NLPAnalysis
            {
                OriginalInput = input,
                DetectedTopic = DetectTopic(input),
                DetectedIntent = DetectIntent(input),
                DetectedAction = DetectAction(input),
                ExtractedTaskName = ExtractTaskName(input),
                Sentiment = AnalyzeSentiment(input),
                AnalysisTimestamp = DateTime.Now
            };
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Check if input contains cybersecurity-related keywords
        public bool IsCybersecurityRelated(string input)
        {
            string topic = DetectTopic(input);
            return !string.IsNullOrEmpty(topic);
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Get suggested responses based on analysis
        public List<string> GetSuggestedResponses(NLPAnalysis analysis)
        {
            var suggestions = new List<string>();

            if (!string.IsNullOrEmpty(analysis.DetectedTopic))
            {
                suggestions.Add($"I can help you with {analysis.DetectedTopic} security.");
                suggestions.Add($"Would you like to know more about {analysis.DetectedTopic}?");
            }

            if (analysis.DetectedIntent == "help_request")
            {
                suggestions.Add("I'm here to help! What specific cybersecurity topic interests you?");
            }

            if (analysis.DetectedIntent == "quiz_request")
            {
                suggestions.Add("Great! Let's test your cybersecurity knowledge with a quiz.");
            }

            if (analysis.Sentiment == "negative")
            {
                suggestions.Add("I understand your concern. Let me help you feel more secure online.");
            }

            return suggestions;
        }
        //---------------------------------------------------------\\
    }
    //----------------------------------------------\\

    //----------------------------------------------\\
    // NLP Analysis result class
    public class NLPAnalysis
    {
        //---------------------------------------------------------\\
        public string OriginalInput { get; set; }
        public string DetectedTopic { get; set; }
        public string DetectedIntent { get; set; }
        public string DetectedAction { get; set; }
        public string ExtractedTaskName { get; set; }
        public string Sentiment { get; set; }
        public DateTime AnalysisTimestamp { get; set; }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        public override string ToString()
        {
            return $"Input: {OriginalInput} | Topic: {DetectedTopic} | Intent: {DetectedIntent} | Action: {DetectedAction} | Sentiment: {Sentiment}";
        }
        //---------------------------------------------------------\\
    }
    //----------------------------------------------\\
}

//------------------------oOo End Of File oOo------------------------\\