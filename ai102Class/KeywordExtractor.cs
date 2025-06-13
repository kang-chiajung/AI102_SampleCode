using Microsoft.Extensions.Configuration;
using OpenAI_API;
using OpenAI_API.Chat;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ai102Class
{
    public class KeywordExtractor
    {
        public static async Task RunAsync()
        {
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

            Console.WriteLine("請輸入要擷取關鍵詞的文字：");
            string input = Console.ReadLine();
            string prompt = $"請從以下文字中擷取三個最具代表性的關鍵詞，並以純 JSON 格式回傳（不要額外說明）：文字：{input} 輸出格式：{{\"keywords\": [\"關鍵詞1\", \"關鍵詞2\", \"關鍵詞3\"]}}";


            var chat = openai.Chat.CreateConversation();
            chat.AppendSystemMessage("你是一個 NLP 助手，會從文字中擷取代表性的關鍵詞並以 JSON 格式回傳。只需回傳 JSON 結果，不需其他說明。");
            chat.AppendUserInput(prompt);

            var response = await chat.GetResponseFromChatbotAsync();

            Console.WriteLine("回傳結果：");
            Console.WriteLine(response);


        }
    }
}
