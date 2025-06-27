// References
// https://chatgpt.com/
// https://www.w3schools.com/cpp/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace PROG6221_POE_ST10444189_GUI
{
    // AudioPlayer class handles playing audio files
    public class AudioPlayer
    {
        //---------------------------------------------------------\\
        private string filePath;
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Constructor to initialize the AudioPlayer with a file path
        public AudioPlayer(string filePath)
        {
            this.filePath = filePath;
        }
        //---------------------------------------------------------\\

        //---------------------------------------------------------\\
        // Method to play the audio file
        public void Play()
        {
            try
            {
                // Initialize the SoundPlayer with the file path
                SoundPlayer player = new SoundPlayer(filePath);
                player.PlaySync(); // PlaySync() waits for completion before continuing
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Error: The audio file was not found. Please check the path. {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error playing audio: " + ex.Message);
            }
        }
        //---------------------------------------------------------\\
    }
}
//------------------------oOo End Of File oOo------------------------\\
