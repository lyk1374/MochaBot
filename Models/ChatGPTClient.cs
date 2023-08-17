using System.Text.Json;
using System.Text;
using System.Diagnostics;

namespace MochaBot.Models
{
    public class ChatGPTClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public ChatGPTClient(string apiKey)
        {
            _httpClient = new HttpClient();
            _apiKey = apiKey;
        }

        public async Task<string> GetChatResponse(string message)
        {
            var requestBody = new
            {
                messages = new[] { new { role = "system", content = message } },
                model = "gpt-3.5-turbo-0613",
                max_tokens = 400
            };

            var requestJson = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var responseJson = await response.Content.ReadAsStringAsync();

            Debug.WriteLine(responseJson);

            var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseJson);

            var chatResponse = jsonResponse
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return chatResponse;
        }
    }
}
