using Microsoft.Extensions.Configuration;
using OpenAI_API;
using OpenAI_API.Chat;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ai102Class
{
    public class DocumentQnA_MultiTurn
    {
        private static Conversation chat;

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

            // 初始化對話一次，保留上下文
            if (chat == null)
            {
                chat = openai.Chat.CreateConversation();
                string document = @"歡迎使用我們的服務。客服時間為週一至週五上午9點至下午6點。出貨時間約為3-5個工作天，使用者可在會員中心查詢物流進度。退貨政策為7日內無條件退貨，請聯繫客服申請。";
                chat.AppendSystemMessage("你是一個智慧客服助理，根據以下 FAQ 內容回答使用者問題，如 FAQ 中沒有提及就回覆 '請洽客服87526388 '：\n\n" + document);
            }

            Console.WriteLine("請輸入您的問題（支援上下文連續問答，輸入 'exit' 離開）：");
            while (true)
            {
                Console.Write("\n> ");
                string question = Console.ReadLine();
                if (question?.ToLower() == "exit") break;

                chat.AppendUserInput(question);
                var response = await chat.GetResponseFromChatbotAsync();

                Console.WriteLine("\n回答結果：");
                Console.WriteLine(response);
            }
        }
    }
}
