using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Service;
using WebApplication2.SpeechService;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpeechController : Controller
    {
        private readonly ISpeechServices _speechService;
        public SpeechController(ISpeechServices speechService)
        {
            _speechService = speechService;
        }

        [HttpPost]
        public void TextToSpeech([FromBody] TextToSpeechResource resource)
        {
            _speechService.TextToSpeech(resource.Message);
        }
    }
}
