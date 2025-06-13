using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ai102Class
{
    public class IdCardIntentRecognizer
    {
        public static void LogFrontInfo(JToken front)
        {
            Console.WriteLine("【正面資料】");
            Console.WriteLine($"  - 身份證字號：{front?["ID"]}");
            Console.WriteLine($"  - 姓名：{front?["NAME"]}");
            Console.WriteLine($"  - 出生日期：{front?["BIRTHDAY"]}（{front?["BIRTHDAY_YYYYMMDD"]}）");
            Console.WriteLine($"  - 性別：{front?["SEX"]}");
            Console.WriteLine($"  - 發證日期：{front?["ISSUED_DATE"]}（{front?["ISSUED_DATE_YYYYMMDD"]}）");
            Console.WriteLine($"  - 發證地點：{front?["ISSUED_PLACE"]}");
            Console.WriteLine($"  - 發證狀態：{front?["ISSUED_STATUS"]}");
        }

        public static async Task RunAsync()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            string apiKey = config["OpenAI:ApiKey"];
            string ocrApiKey = config["OpenAI:OcrApiKey"];
            Console.Write("請輸入身份證圖片檔案路徑：");
            string filePath = Console.ReadLine();
       
            if (!File.Exists(filePath))
            {
                Console.WriteLine("⚠️ 找不到指定的圖片檔案。請確認路徑。");
                return;
            }

            Console.WriteLine("請描述你想對這張身份證做什麼？（例如：幫我辨識這張身份證的內容）");
            string intentInput = Console.ReadLine();

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var chatPayload = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = "你是一個語意分析助理，會根據使用者的輸入判斷是否需要進行身份證 OCR 分析。" },
                    new { role = "user", content = intentInput }
                }
            };

            var chatContent = new StringContent(JsonSerializer.Serialize(chatPayload));
            chatContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var chatResponse = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", chatContent);
            var chatResult = await chatResponse.Content.ReadAsStringAsync();

            if (!chatResult.ToLower().Contains("ocr") && !chatResult.ToLower().Contains("辨識"))
            {
                Console.WriteLine("❌ 使用者意圖不包含進行身份證辨識。");
                return;
            }

            Console.WriteLine("✅ 判斷為需要 OCR，開始上傳圖片至後端 API...");
            string username = "FOC_O15"; // ← 可從設定檔讀取
            string password = ocrApiKey;
            string credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{username}:{password}"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            var requestContent = new MultipartFormDataContent();
            var imageContent = new ByteArrayContent(File.ReadAllBytes(filePath));
            imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            requestContent.Add(imageContent, "file", Path.GetFileName(filePath));

           // var ocrResponse = await httpClient.PostAsync("https://ocr-center-sit.chailease.com.tw/ocr-center-service/api/IdCard/v1/Recognize/Front", requestContent);
          //  var ocrResult = await ocrResponse.Content.ReadAsStringAsync();
            var json = JObject.Parse(@"
{
    ""Data"": {
        ""SN"": ""ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff"",
        ""NewFileName"": ""FAKE-ID-FRONT.jpg"",
        ""IdrType"": ""身份證正面"",
        ""IdentityCard"": {
            ""front"": {
                ""ID"": ""Z987654321"",
                ""NAME"": ""陳x君"",
                ""BIRTHDAY"": ""民國85年5月20日"",
                ""BIRTHDAY_YYYYMMDD"": ""19960520"",
                ""ISSUED_DATE"": ""民國110年3月10日(新北市)初發"",
                ""ISSUED_STATUS"": ""初發"",
                ""ISSUED_PLACE"": ""新北市"",
                ""SEX"": ""女"",
                ""ISSUED_DATE_YYYYMMDD"": ""20210310""
            },
            ""back"": null
        },
        ""HealthInsuranceCard"": null
    },
    ""Result"": {
        ""ReturnCode"": 0,
        ""ReturnMsg"": ""辨識成功（假資料）"",
        ""Alert"": """",
        ""Result"": 0
    }
}

");
            var idCard = json["Data"]?["IdentityCard"];
            Console.WriteLine("\n🔍 OCR 辨識結果：");
            LogFrontInfo(idCard?["front"]);
            //Console.WriteLine(ocrResult);
        }
    }
}
