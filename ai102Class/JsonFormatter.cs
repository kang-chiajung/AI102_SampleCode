using Microsoft.Extensions.Configuration;
using OpenAI_API;
using OpenAI_API.Chat;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ai102Class
{
    public class JsonFormatter
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

            Console.WriteLine("請輸入要轉換成 JSON 的自然語言描述：");
            string input = Console.ReadLine();

            string prompt = $"請將以下自然語言描述轉換成 JSON 格式，僅輸出純 JSON，避免額外說明：\n\n\"{input}\"";

            var chat = openai.Chat.CreateConversation();
            chat.AppendSystemMessage("你是一個結構化資料助手，將自然語言轉為 JSON 結構，僅回傳 JSON 結果。例：\"姓名是王小明，電話是0912-345678\" → { \"name\": \"王小明\", \"phone\": \"0912-345678\" }");
            chat.AppendUserInput(prompt);

            var response = await chat.GetResponseFromChatbotAsync();

            Console.WriteLine("回傳結果：");
            Console.WriteLine(response);
        }
    }
}
