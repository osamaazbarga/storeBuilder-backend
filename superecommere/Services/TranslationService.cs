using Newtonsoft.Json;
using System.Text;

namespace superecommere.Services
{
    public class TranslationService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "YOUR_GOOGLE_TRANSLATE_API_KEY"; // قم بوضع API Key الخاص بك

        public TranslationService(IConfiguration config)
        {
            _httpClient = new HttpClient();
            _config = config;
        }

        public async Task<string> TranslateTextAsync(string text, string targetLanguage)
        {
            try
            {
                var url = $"https://translation.googleapis.com/language/translate/v2?key={_config["Google:ApiKey"]}";

                var body = new
                {
                    q = text,
                    target = targetLanguage
                };

                var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(responseContent);
                    return result.data.translations[0].translatedText;
                }
                else
                {
                    return "Error: " + response.ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                return "Exception: " + ex.Message;
            }
        }
    }
}
