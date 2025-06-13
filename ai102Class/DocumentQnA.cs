using Microsoft.Extensions.Configuration;
using OpenAI_API;
using OpenAI_API.Chat;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ai102Class
{
    public class DocumentQnA
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

            string document = @"歡迎使用我們的服務。客服時間為週一至週五上午9點至下午6點。出貨時間約為3-5個工作天，使用者可在會員中心查詢物流進度。退貨政策為7日內無條件退貨，請聯繫客服申請。";

            Console.WriteLine("請輸入您的問題（會根據內建的文件回答）：");
            string question = Console.ReadLine();

            string prompt = $"請根據以下文件內容回答使用者問題，如無相關資訊請回答\"文件中未提及\"：\n文件：{document}\n\n問題：{question}";

            var chat = openai.Chat.CreateConversation();
            chat.AppendSystemMessage("你是一個智慧客服助理，根據給定的文件內容回答問題，無需額外延伸推論。");
            chat.AppendUserInput(prompt);

            var response = await chat.GetResponseFromChatbotAsync();

            Console.WriteLine("回答結果：");
            Console.WriteLine(response);
        }
    }
}
