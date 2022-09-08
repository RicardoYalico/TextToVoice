using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.SpeechService
{
    public interface ISpeechServices
    {
        public void TextToSpeech(String message);
    }
}
