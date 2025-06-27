using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace PROG6221_POE_ST10444189_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //---------------------------------------------------------\\
        private ChatBot bot = new ChatBot(); // Instance of your chatbot
        private AudioPlayer audioPlayer; // Instance for audio playback
        private AsciiArt asciiArt; // Instance for ASCII art
        private bool hasPlayedWelcomeAudio = false; // Track if welcome audio has played
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        public MainWindow()
        {
            InitializeComponent();
            InitializeComponents();
            DisplayWelcomeMessage();
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        /// <summary>
        /// Initialize audio player and ASCII art components
        /// </summary>
        private void InitializeComponents()
        {
            try
            {
                // Initialize audio player with a default notification sound path
                string audioPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "greeting.wav");
                audioPlayer = new AudioPlayer(audioPath);

                // Only play welcome audio once during initialization
                if (!hasPlayedWelcomeAudio)
                {
                    audioPlayer.Play();
                    hasPlayedWelcomeAudio = true;
                }

                // Initialize ASCII art
                string asciiPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "ascii_art.txt");
                asciiArt = new AsciiArt(asciiPath);
            }
            catch (Exception ex)
            {
                // If initialization fails, create with empty paths to avoid null reference
                audioPlayer = new AudioPlayer("");
                asciiArt = new AsciiArt("");

                // Optionally log the error
                AddMessageToChat("System", $"Warning: Could not initialize audio/art resources: {ex.Message}");
            }
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        /// <summary>
        /// Display welcome message and ASCII art in the chat window
        /// </summary>
        private void DisplayWelcomeMessage()
        {
            try
            {
                // Add welcome banner to chat
                AddMessageToChat("SecureBot", "╔══════════════════════════════════════════╗");
                AddMessageToChat("SecureBot", "║         👋 Welcome to SecureBot          ║");
                AddMessageToChat("SecureBot", "╠══════════════════════════════════════════╣");
                AddMessageToChat("SecureBot", "║   Ask me anything about cyber security!  ║");
                AddMessageToChat("SecureBot", "╚══════════════════════════════════════════╝");
                AddMessageToChat("SecureBot", "");

                // Try to display ASCII art from file
                DisplayAsciiArt();

                // Add initial bot greeting with enhanced features
                AddMessageToChat("SecureBot", "Hello! I'm SecureBot, your enhanced cybersecurity assistant.");
                AddMessageToChat("SecureBot", "🔹 Ask me about passwords, phishing, privacy, scams, malware, and more!");
                AddMessageToChat("SecureBot", "🔹 Take a cybersecurity quiz to test your knowledge");
                AddMessageToChat("SecureBot", "🔹 Create cybersecurity tasks with reminders");
                AddMessageToChat("SecureBot", "🔹 View your activity log and learning progress");
                AddMessageToChat("SecureBot", "To get started, try saying 'My name is [YourName]' or ask about any cybersecurity topic!");
                AddMessageToChat("SecureBot", "Type 'help' to see all available commands.");
            }
            catch (Exception ex)
            {
                AddMessageToChat("System", $"Error displaying welcome message: {ex.Message}");
            }
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        /// <summary>
        /// Display ASCII art from file in the chat window
        /// </summary>
        private void DisplayAsciiArt()
        {
            try
            {
                string asciiPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "ascii_art.txt");

                if (File.Exists(asciiPath))
                {
                    string asciiContent = File.ReadAllText(asciiPath);
                    string[] lines = asciiContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string line in lines)
                    {
                        AddMessageToChat("ASCII", line);
                    }
                }
                else
                {
                    AddMessageToChat("SecureBot", "🎨 [ASCII Art would appear here if ascii_art.txt was found in Resources folder]");
                }

                AddMessageToChat("SecureBot", ""); // Add spacing after ASCII art
            }
            catch (Exception ex)
            {
                AddMessageToChat("System", $"Could not display ASCII art: {ex.Message}");
            }
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        /// <summary>
        /// Handle sending messages when Send button is clicked
        /// </summary>
        private void SendMessage(object sender, RoutedEventArgs e)
        {
            string userMessage = UserInput.Text.Trim();

            if (!string.IsNullOrEmpty(userMessage))
            {
                // Display user input in chat
                AddMessageToChat("You", userMessage);

                // Get bot response
                string botResponse = bot.Respond(userMessage);

                // Handle multi-line responses (like task lists, activity logs)
                string[] responseLines = botResponse.Split(new[] { '\n' }, StringSplitOptions.None);

                if (responseLines.Length > 1)
                {
                    // Multi-line response - display each line separately for better formatting
                    foreach (string line in responseLines)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            AddMessageToChat("SecureBot", line.Trim());
                        }
                        else
                        {
                            // Add empty line for spacing
                            ChatHistory.Items.Add("");
                        }
                    }
                }
                else
                {
                    // Single line response
                    AddMessageToChat("SecureBot", botResponse);
                }

                // Play notification sound for bot response
                PlayNotificationSound();

                // Clear input field
                UserInput.Clear();

                // Set focus back to input for continuous conversation
                UserInput.Focus();

                // Auto-scroll to bottom to show latest messages
                ScrollToBottom();
            }
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        /// <summary>
        /// Handle Enter key press in text input
        /// </summary>
        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage(sender, new RoutedEventArgs());
            }
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        /// <summary>
        /// Add a message to the chat history with sender identification
        /// </summary>
        private void AddMessageToChat(string sender, string message)
        {
            try
            {
                string timestamp = DateTime.Now.ToString("HH:mm");
                string formattedMessage;

                // Format message based on sender
                switch (sender.ToLower())
                {
                    case "you":
                        formattedMessage = $"[{timestamp}] 👤 You: {message}";
                        break;
                    case "securebot":
                        formattedMessage = $"[{timestamp}] 🤖 SecureBot: {message}";
                        break;
                    case "ascii":
                        formattedMessage = message; // ASCII art without formatting
                        break;
                    case "system":
                        formattedMessage = $"[{timestamp}] ⚠️ System: {message}";
                        break;
                    default:
                        formattedMessage = $"[{timestamp}] {sender}: {message}";
                        break;
                }

                ChatHistory.Items.Add(formattedMessage);
            }
            catch (Exception ex)
            {
                // Fallback if formatting fails
                ChatHistory.Items.Add($"{sender}: {message}");
            }
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        /// <summary>
        /// Play notification sound when bot responds (only for regular responses, not welcome)
        /// </summary>
        private void PlayNotificationSound()
        {
            try
            {
                // Don't play notification sound for help command or during initialization
                // Only play for actual conversational responses
            }
            catch (Exception ex)
            {
                // Silently handle audio errors to not disrupt chat flow
                Console.WriteLine($"Audio playback error: {ex.Message}");
            }
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        /// <summary>
        /// Auto-scroll chat history to bottom to show latest messages
        /// </summary>
        private void ScrollToBottom()
        {
            try
            {
                if (ChatHistory.Items.Count > 0)
                {
                    ChatHistory.ScrollIntoView(ChatHistory.Items[ChatHistory.Items.Count - 1]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Scroll error: {ex.Message}");
            }
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        /// <summary>
        /// Window loaded event - set focus to input field
        /// </summary>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UserInput.Focus();
        }
        //---------------------------------------------------------\\
    }
}

//------------------------oOo End Of File oOo------------------------\\