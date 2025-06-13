using System;
using System.Threading.Tasks;

namespace ai102Class
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
              
                Console.Clear();
                Console.WriteLine("=== AI-102 課程功能選單 ===");
                Console.WriteLine("中台科-家榮");
                Console.WriteLine("1. 情緒分析");
                Console.WriteLine("2. 關鍵詞擷取");
                Console.WriteLine("3. 語意意圖分類（查詢 / 投訴 / 感謝）");  
                Console.WriteLine("4. 自然語言轉 JSON 結構化");
                Console.WriteLine("5. 文件問答（模擬 FAQ 、問答）");
                Console.WriteLine("6. 語音處理轉文字");
                Console.WriteLine("7. 語意理解 + 進行證件辦識");
                Console.WriteLine("q. 離開");
                Console.Write("\n請輸入選項：");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await SentimentAnalyzer.RunAsync();
                        break;
                    case "2":
                        //這次的網購經驗真的很糟糕，商品延遲了快一週才到貨，而且客服完全沒有處理問題的誠意。
                        await KeywordExtractor.RunAsync();
                        break;
                    case "3":
                        //請問這雙鞋還有貨嗎？
                        await IntentClassifier.RunAsync();
                        break;
                    case "4":
                        //我叫王小明，電話是0912-345678，想預約6月20日下午2點看診。
                        
                        await JsonFormatter.RunAsync();
                        break;
                    case "5":
                        //await DocumentQnA.RunAsync();
                        await DocumentQnA_MultiTurn.RunAsync();
                        break;
                    case "6":
                        await SpeechModule.RunAsync();
                        break;
                    case "7":
                        await IdCardIntentRecognizer.RunAsync();
                        break;
                    case "q":
                    case "Q":
                        Console.WriteLine("再見！");
                        return;
                    default:
                        Console.WriteLine("❌ 無效選項，請重新輸入");
                        break;
                }

                Console.WriteLine("\n按任意鍵返回主選單...");
                Console.ReadKey();
            }
        }
    }
}