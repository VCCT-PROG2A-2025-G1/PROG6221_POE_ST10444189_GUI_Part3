using System;
using System.Collections.Generic;
using System.Linq;

namespace PROG6221_POE_ST10444189_GUI
{
    //----------------------------------------------\\
    // Quiz Question Class
    public class QuizQuestion
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public string Explanation { get; set; }
        public bool IsTrueFalse { get; set; }

        public QuizQuestion()
        {
            Options = new List<string>();
        }
    }
    //----------------------------------------------\\

    //----------------------------------------------\\
    // Cybersecurity Quiz Class
    public class CybersecurityQuiz
    {
        private List<QuizQuestion> questions;
        private int currentQuestionIndex;
        private int correctAnswers;
        private int totalQuestions;
        private bool isQuizActive;
        private Random random;

        public CybersecurityQuiz()
        {
            questions = new List<QuizQuestion>();
            random = new Random();
            InitializeQuestions();
            ResetQuiz();
        }

        //----------------------------------------------\\
        // Initialize all quiz questions
        private void InitializeQuestions()
        {
            // Question 1 - Multiple Choice
            questions.Add(new QuizQuestion
            {
                Question = "What should you do if you receive an email asking for your password?",
                Options = new List<string> { "A) Reply with your password", "B) Delete the email", "C) Report the email as phishing", "D) Ignore it" },
                CorrectAnswerIndex = 2,
                Explanation = "Correct! Reporting phishing emails helps prevent scams and protects others from falling victim to the same attack.",
                IsTrueFalse = false
            });

            // Question 2 - True/False
            questions.Add(new QuizQuestion
            {
                Question = "True or False: It's safe to use the same password for multiple accounts if it's very strong.",
                Options = new List<string> { "True", "False" },
                CorrectAnswerIndex = 1,
                Explanation = "False! Even strong passwords should be unique for each account. If one account is compromised, all your accounts become vulnerable.",
                IsTrueFalse = true
            });

            // Question 3 - Multiple Choice
            questions.Add(new QuizQuestion
            {
                Question = "Which of these is the strongest password?",
                Options = new List<string> { "A) password123", "B) MyDog2023", "C) Tr@il$Hiking#47", "D) 12345678" },
                CorrectAnswerIndex = 2,
                Explanation = "Correct! Strong passwords use a mix of uppercase, lowercase, numbers, and special characters with sufficient length.",
                IsTrueFalse = false
            });

            // Question 4 - True/False
            questions.Add(new QuizQuestion
            {
                Question = "True or False: You should immediately click links in urgent emails from your bank.",
                Options = new List<string> { "True", "False" },
                CorrectAnswerIndex = 1,
                Explanation = "False! Legitimate banks rarely send urgent emails with links. Always verify by logging into your account directly or calling the bank.",
                IsTrueFalse = true
            });

            // Question 5 - Multiple Choice
            questions.Add(new QuizQuestion
            {
                Question = "What does 'HTTPS' indicate about a website?",
                Options = new List<string> { "A) It's a government website", "B) It has encrypted communication", "C) It's free to use", "D) It loads faster" },
                CorrectAnswerIndex = 1,
                Explanation = "Correct! HTTPS means the communication between your browser and the website is encrypted, making it safer for sensitive information.",
                IsTrueFalse = false
            });

            // Question 6 - Multiple Choice
            questions.Add(new QuizQuestion
            {
                Question = "What should you do before downloading software from the internet?",
                Options = new List<string> { "A) Download from any website", "B) Verify the source is trustworthy", "C) Disable antivirus software", "D) Share it with friends first" },
                CorrectAnswerIndex = 1,
                Explanation = "Correct! Always download software from official websites or trusted sources to avoid malware and viruses.",
                IsTrueFalse = false
            });

            // Question 7 - True/False
            questions.Add(new QuizQuestion
            {
                Question = "True or False: Public Wi-Fi networks are always safe for online banking.",
                Options = new List<string> { "True", "False" },
                CorrectAnswerIndex = 1,
                Explanation = "False! Public Wi-Fi networks are often unsecured. Avoid accessing sensitive accounts on public networks, or use a VPN for protection.",
                IsTrueFalse = true
            });

            // Question 8 - Multiple Choice
            questions.Add(new QuizQuestion
            {
                Question = "What is social engineering in cybersecurity?",
                Options = new List<string> { "A) Building social media websites", "B) Manipulating people to reveal information", "C) Creating engineering software", "D) Networking with engineers" },
                CorrectAnswerIndex = 1,
                Explanation = "Correct! Social engineering involves manipulating people psychologically to divulge confidential information or perform actions that compromise security.",
                IsTrueFalse = false
            });

            // Question 9 - True/False
            questions.Add(new QuizQuestion
            {
                Question = "True or False: Antivirus software alone provides complete protection against all cyber threats.",
                Options = new List<string> { "True", "False" },
                CorrectAnswerIndex = 1,
                Explanation = "False! While antivirus is important, complete cybersecurity requires multiple layers including safe browsing habits, strong passwords, and regular updates.",
                IsTrueFalse = true
            });

            // Question 10 - Multiple Choice
            questions.Add(new QuizQuestion
            {
                Question = "How often should you update your software and operating system?",
                Options = new List<string> { "A) Never", "B) Once a year", "C) As soon as updates are available", "D) Only when it stops working" },
                CorrectAnswerIndex = 2,
                Explanation = "Correct! Regular updates patch security vulnerabilities and protect against new threats. Enable automatic updates when possible.",
                IsTrueFalse = false
            });

            // Question 11 - Multiple Choice (Bonus)
            questions.Add(new QuizQuestion
            {
                Question = "What does two-factor authentication (2FA) provide?",
                Options = new List<string> { "A) Faster login", "B) Extra security layer", "C) Better passwords", "D) Free account upgrades" },
                CorrectAnswerIndex = 1,
                Explanation = "Correct! 2FA adds an extra layer of security by requiring something you know (password) and something you have (phone/token).",
                IsTrueFalse = false
            });

            // Question 12 - True/False (Bonus)
            questions.Add(new QuizQuestion
            {
                Question = "True or False: It's safe to leave your devices unlocked in public places if you're just stepping away briefly.",
                Options = new List<string> { "True", "False" },
                CorrectAnswerIndex = 1,
                Explanation = "False! Always lock your devices when unattended. It only takes seconds for someone to access your personal information or install malware.",
                IsTrueFalse = true
            });
        }
        //----------------------------------------------\\

        //----------------------------------------------\\
        // Start a new quiz
        public string StartQuiz()
        {
            ResetQuiz();
            isQuizActive = true;
            // Shuffle questions for variety
            ShuffleQuestions();
            totalQuestions = Math.Min(10, questions.Count); // Limit to 10 questions
            return "🎯 Welcome to the Cybersecurity Quiz! 🎯\n\n" +
                   $"I'll ask you {totalQuestions} questions to test your cybersecurity knowledge.\n" +
                   "Type 'A', 'B', 'C', 'D' for multiple choice or 'True'/'False' for true/false questions.\n\n" +
                   GetCurrentQuestion();
        }
        //----------------------------------------------\\

        //----------------------------------------------\\
        // Process user's answer
        public string ProcessAnswer(string userAnswer)
        {
            if (!isQuizActive)
                return "No quiz is currently active. Type 'start quiz' to begin!";

            if (currentQuestionIndex >= totalQuestions)
                return "Quiz has ended! Type 'start quiz' to play again.";

            string answer = userAnswer.Trim().ToUpper();
            QuizQuestion currentQuestion = questions[currentQuestionIndex];
            bool isCorrect = false;
            string response = "";

            // Check if answer is correct
            if (currentQuestion.IsTrueFalse)
            {
                if (answer == "TRUE" || answer == "T")
                    isCorrect = (currentQuestion.CorrectAnswerIndex == 0);
                else if (answer == "FALSE" || answer == "F")
                    isCorrect = (currentQuestion.CorrectAnswerIndex == 1);
                else
                    return "Please answer with 'True' or 'False'.";
            }
            else
            {
                if (answer == "A" && currentQuestion.CorrectAnswerIndex == 0) isCorrect = true;
                else if (answer == "B" && currentQuestion.CorrectAnswerIndex == 1) isCorrect = true;
                else if (answer == "C" && currentQuestion.CorrectAnswerIndex == 2) isCorrect = true;
                else if (answer == "D" && currentQuestion.CorrectAnswerIndex == 3) isCorrect = true;
                else if (!new[] { "A", "B", "C", "D" }.Contains(answer))
                    return "Please answer with A, B, C, or D.";
            }

            // Update score
            if (isCorrect)
            {
                correctAnswers++;
                response = "✅ " + currentQuestion.Explanation;
            }
            else
            {
                string correctOption = currentQuestion.IsTrueFalse ?
                    currentQuestion.Options[currentQuestion.CorrectAnswerIndex] :
                    currentQuestion.Options[currentQuestion.CorrectAnswerIndex];
                response = $"❌ Incorrect. The correct answer is: {correctOption}\n" + currentQuestion.Explanation;
            }

            currentQuestionIndex++;

            // Check if quiz is complete
            if (currentQuestionIndex >= totalQuestions)
            {
                isQuizActive = false;
                response += "\n\n" + GetFinalScore();
            }
            else
            {
                response += $"\n\nScore: {correctAnswers}/{currentQuestionIndex}\n\n" + GetCurrentQuestion();
            }

            return response;
        }
        //----------------------------------------------\\

        //----------------------------------------------\\
        // Get current question
        private string GetCurrentQuestion()
        {
            if (currentQuestionIndex >= questions.Count)
                return "No more questions available.";

            QuizQuestion question = questions[currentQuestionIndex];
            string questionText = $"Question {currentQuestionIndex + 1}/{totalQuestions}:\n{question.Question}\n\n";

            for (int i = 0; i < question.Options.Count; i++)
            {
                questionText += question.Options[i] + "\n";
            }

            return questionText;
        }
        //----------------------------------------------\\

        //----------------------------------------------\\
        // Get final score and feedback
        private string GetFinalScore()
        {
            double percentage = (double)correctAnswers / totalQuestions * 100;
            string scoreText = $"🏆 Quiz Complete! 🏆\nFinal Score: {correctAnswers}/{totalQuestions} ({percentage:F0}%)\n\n";

            if (percentage >= 90)
                scoreText += "🌟 Outstanding! You're a cybersecurity expert! Keep up the excellent work!";
            else if (percentage >= 80)
                scoreText += "🎉 Great job! You have strong cybersecurity knowledge!";
            else if (percentage >= 70)
                scoreText += "👍 Good work! You're on the right track to staying safe online!";
            else if (percentage >= 60)
                scoreText += "📚 Not bad! Keep learning to improve your cybersecurity awareness!";
            else
                scoreText += "💪 Keep studying! Cybersecurity knowledge is crucial for staying safe online. Try again to improve your score!";

            scoreText += "\n\nType 'start quiz' to play again and test your knowledge!";
            return scoreText;
        }
        //----------------------------------------------\\

        //----------------------------------------------\\
        // Shuffle questions for variety
        private void ShuffleQuestions()
        {
            for (int i = 0; i < questions.Count; i++)
            {
                int randomIndex = random.Next(i, questions.Count);
                QuizQuestion temp = questions[i];
                questions[i] = questions[randomIndex];
                questions[randomIndex] = temp;
            }
        }
        //----------------------------------------------\\

        //----------------------------------------------\\
        // Reset quiz variables
        private void ResetQuiz()
        {
            currentQuestionIndex = 0;
            correctAnswers = 0;
            isQuizActive = false;
        }
        //----------------------------------------------\\

        //----------------------------------------------\\
        // Check if quiz is active
        public bool IsQuizActive()
        {
            return isQuizActive;
        }
        //----------------------------------------------\\

        //----------------------------------------------\\
        // Get quiz status
        public string GetQuizStatus()
        {
            if (!isQuizActive)
                return "No quiz is currently active. Type 'start quiz' to begin the cybersecurity quiz!";

            return $"Quiz in progress - Question {currentQuestionIndex + 1}/{totalQuestions}\nCurrent Score: {correctAnswers}/{currentQuestionIndex}";
        }
        //----------------------------------------------\\
    }
}

//------------------------oOo End Of Quiz Implementation oOo------------------------\\