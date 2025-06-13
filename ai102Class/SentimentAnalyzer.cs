using Microsoft.Extensions.Configuration;
using OpenAI_API;
using OpenAI_API.Chat;

namespace ai102Class
{
    public class SentimentAnalyzer
    {
        public static async Task RunAsync()
        {
            // 建立設定讀取器，讀取 appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            string apiKey = config["OpenAI:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("⚠️ 無法讀取 API 金鑰，請確認 appsettings.json 設定。");
                return;
            }

            var openai = new OpenAIAPI(apiKey);

            Console.WriteLine("請輸入要分析的文字：");
            string input = Console.ReadLine();

            string prompt = $@"
            請分析以下文字的情緒（正向、負向或中立、非常負向），並擷取3個最能代表內容的關鍵詞，請以 JSON 格式回傳，格式如下：
            {{
              ""sentiment"": ""正向"",
              ""keywords"": [""關鍵詞1"", ""關鍵詞2"", ""關鍵詞3""]
            }}
            文字如下：
            {input}
            ";

            var chat = openai.Chat.CreateConversation();
            chat.AppendSystemMessage("你是一個語意分析助手，會以 JSON 格式輸出結果。");
            chat.AppendUserInput(prompt);

            var response = await chat.GetResponseFromChatbotAsync();

            Console.WriteLine("回傳結果：");
            Console.WriteLine(response);
        }
    }
}
