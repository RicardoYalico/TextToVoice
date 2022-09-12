using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.Extensions.Configuration;
using WebApplication2.SpeechService;


namespace WebApplication2.Service
{

    public class SpeechServices : ISpeechServices
    {
        // Interface para obtener las variables de la configuración del appsetting
        public IConfiguration Configuration { get; }
        // Credenciales Speech para utilizar el servicio
        static string YourSubscriptionKey;
        static string YourServiceRegion;
        public SpeechServices(IConfiguration configuration)
        {
            Configuration = configuration;
            YourSubscriptionKey = Configuration.GetSection("YourSubscriptionKey").Value;
            YourServiceRegion = Configuration.GetSection("YourServiceRegion").Value;
        }

        // Función para utilizar el servicio
        public async void TextToSpeech(string message)
        {
            // Obtenemos los valores de la configuración
            string path = Configuration.GetSection("PathToSave").Value;
            string voice = Configuration.GetSection("Voice").Value;
            string rate = Configuration.GetSection("Rate").Value;
            string volume = Configuration.GetSection("Volume").Value;

            // Create the file, or overwrite if the file exists.
            using (FileStream fs = File.Create(path))
            {
                byte[] info = new UTF8Encoding(true).GetBytes("This is some text in the file.");
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }


            var speechConfig = SpeechConfig.FromSubscription(YourSubscriptionKey, YourServiceRegion);

            // Output Format
            speechConfig.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Riff8Khz16BitMonoPcm);
            // The language of the voice that speaks.
            speechConfig.SpeechSynthesisVoiceName = voice;
            
            using (var speechSynthesizer = new SpeechSynthesizer(speechConfig))
            {
                // Se arma la estructura de cuerpo en SSML
                string str = "<speak version=\"1.0\"";
                str += " xmlns=\"http://www.w3.org/2001/10/synthesis\"";
                str += " xml:lang=\"en-US\">";
                str += "<voice name='";
                str += voice;
                str += "'> ";
                str += "<prosody rate='";
                str += rate;
                str += "' volume='";
                str += volume;
                //str += "' contour='(80%,-30%) (100%,+80%)";
                str += "'>";
                str += message;
                str += "</prosody>";
                str += "</voice>";
                str += "</speak>";

                // Primero habla el bot y luego se almacena
                var speechSynthesisResult = await speechSynthesizer.SpeakSsmlAsync(str);

                // Guardado del audio en la ruta indicada
                var stream = AudioDataStream.FromResult(speechSynthesisResult);
                await stream.SaveToWaveFileAsync(path);
            }
        }
    }
}
