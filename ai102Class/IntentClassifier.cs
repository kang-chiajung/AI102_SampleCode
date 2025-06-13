using Microsoft.Extensions.Configuration;
using OpenAI_API;
using OpenAI_API.Chat;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ai102Class
{
    public class IntentClassifier
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

            Console.WriteLine("請輸入要分類的語句（查詢 / 投訴 / 感謝）：");
            string input = Console.ReadLine();

            string prompt = $"請判斷以下語句的意圖類別（查詢、投訴或感謝），並以 JSON 格式回傳，格式如下：\n{{\n  \"intent\": \"查詢\"\n}}\n語句如下：{input}";

            var chat = openai.Chat.CreateConversation();
            chat.AppendSystemMessage("你是一個客服分類助手，會根據語句內容判斷使用者的意圖，僅回傳 intent 字段的 JSON 結果。");
            chat.AppendUserInput(prompt);

            var response = await chat.GetResponseFromChatbotAsync();

            Console.WriteLine("回傳結果：");
            Console.WriteLine(response);
        }
    }
}
