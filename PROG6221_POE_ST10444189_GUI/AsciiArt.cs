using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG6221_POE_ST10444189_GUI
{ // AsciiArt class handles displaying ASCII art and the welcome screen
    public class AsciiArt
    {
        //---------------------------------------------------------\\
        private string filePath;
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Constructor to initialize the AsciiArt with a file path
        public AsciiArt(string filePath)
        {
            this.filePath = filePath;
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Method to display the formatted welcome screen
        public void ShowWelcomeScreen()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta; // Set text color to magenta for the welcome banner
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║         👋 Welcome to SecureBot          ║");
            Console.WriteLine("╠══════════════════════════════════════════╣");
            Console.WriteLine("║   Ask me anything about cyber security!  ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");
            Console.ResetColor(); // Reset text color to default after printing the banner
            Console.WriteLine();
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Method to display the ASCII art from the file
        public void Display()
        {
            // Use a fixed file path for ASCII art
            string defaultFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "ascii_art.txt");
            // Check if the file exists before trying to read it
            if (File.Exists(defaultFilePath))
            {
                // Read all text from the file
                string asciiArt = File.ReadAllText(defaultFilePath);
                // Change the console text color to green for the ASCII art
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(asciiArt);
                Console.ResetColor(); // Reset the console color back to default
            }
            else
            {
                // If the file is not found, print an error message
                Console.WriteLine("Error: ASCII art file not found. Make sure the path is correct.");
            }
        }
        //---------------------------------------------------------\\
    }
}

//------------------------oOo End Of File oOo------------------------\\

