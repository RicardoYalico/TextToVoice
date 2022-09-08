using System;
using System.IO;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using System.Text;
using WebApplication2.SpeechService;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Security.AccessControl;
using System.Globalization;

public enum Voices
{
    Helena= 1,
    Laura = 2,
    Pablo = 3,
    Raul = 4,
    Sabina = 5,
}

namespace WebApplication2.Service
{
    public class SpeechServices : ISpeechServices
    {
        public IConfiguration Configuration { get; }


        public SpeechServices(IConfiguration configuration)
        {
            Configuration = configuration;

        }


        public void TextToSpeech(String message)
        {
            // GET values 
            string path = Configuration.GetSection("PathToSave").Value;
            int voice = Int32.Parse(Configuration.GetSection("Voice").Value);

            // Create the file, or overwrite if the file exists.
            using (FileStream fs = File.Create(path))
            {
                byte[] info = new UTF8Encoding(true).GetBytes("This is some text in the file.");
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }

            // Initialize a new instance of the SpeechSynthesizer.
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {

                //synth.SelectVoice("Microsoft Raul Desktop");

                synth.SelectVoice("Microsoft "+((Voices)voice).ToString());


                // Configure the audio output.   
                synth.SetOutputToWaveFile(path,
                  new SpeechAudioFormatInfo(8000, AudioBitsPerSample.Sixteen, AudioChannel.Mono));
                // Create a SoundPlayer instance to play output audio file.  
                System.Media.SoundPlayer m_SoundPlayer =
                  new System.Media.SoundPlayer(path);

                // Build a prompt.  
                PromptBuilder builder = new PromptBuilder();
                builder.AppendText(message);
                

                // Speak the prompt.  
                synth.Speak(builder);
                
                m_SoundPlayer.Play();
            }

        }



    }
}
