using Microsoft.AspNetCore.Mvc;
using MochaBot.Models;
using System.Diagnostics;

namespace MochaBot.Controllers
{
    public class MochaController : Controller
    {

        private readonly ChatGPTClient _chatGPTClient;

        public MochaController()
        {
            var apiKey = "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";
            _chatGPTClient = new ChatGPTClient(apiKey);
        }



        public async Task<IActionResult> GetDataAsync(string writer, string content)
        {     
            MochaBotModel mbm = new MochaBotModel();

            string timeStamp = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

            int timeLeft = 0;
            bool isNew = mbm.ChkIsNew(writer, timeStamp);

            if (isNew == false)
            { 
                timeLeft = 5 - mbm.ChkTimeStamp(writer, timeStamp);
            }

            if (writer.Equals("김예원"))
            {
                timeLeft = 0;
            }

            if (timeLeft > 0)
            {
                return Content(writer + "님께 답장: 질문은 5분에 하나! "+timeLeft+"분 남음!");
            }

            string reply = string.Empty;
            Debug.WriteLine(writer + ": " + content);

            string frontChk = content.Substring(0, 2);
            string input = content.Replace("$$", "");

            if(frontChk.Equals("$$"))
            {

                var response = await _chatGPTClient.GetChatResponse(input);
                Debug.WriteLine(response);
                reply = writer + "님께 답장: ";
                reply += response.ToString();
            }
            return Content(reply);
        }
    }
}
