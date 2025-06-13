# AI-102 課程分享專案

本專案為 AI-102 課程的實作範例，整合 OpenAI GPT 模型與 Whisper 模型，展示如何在 .NET (C#) 環境下應用 AI 完成語意理解、語音辨識、問答系統等實用任務。

---

## 📘 課程功能選單

Console 應用會顯示以下功能供選擇：

```
=== AI-102 課程功能選單 ===
中台科-家榮
1. 情緒分析
2. 關鍵詞擷取
3. 語意意圖分類（查詢 / 投訴 / 感謝）
4. 自然語言轉 JSON 結構化
5. 文件問答（模擬 FAQ 、問答）
6. 語音處理轉文字
7. 語意理解 + 進行證件辦識
q. 離開
```

---

## 🔧 執行說明

1. 安裝 .NET 6 SDK 或更新版本。
2. 於根目錄建立 `appsettings.json`，內容如下（請填入你自己的 API 金鑰）：

```json
{
  "OpenAI": {
    "ApiKey": "your-api-key-here"
  }
}
```

3. 執行專案：
```bash
dotnet run --project ai102Class
```

---

## 📁 專案結構說明

| 檔案/資料夾             | 說明                              |
|--------------------------|-----------------------------------|
| `SpeechModule.cs`        | 使用 OpenAI Whisper 語音轉文字   |
| `JsonFormatter.cs`       | 自然語言轉 JSON 結構              |
| `FaqModule.cs`           | 問答模組（支援 FAQ）             |
| `EmotionAnalyzer.cs`     | 情緒分析模組                      |
| `IntentClassifier.cs`    | 意圖分類（查詢、感謝、投訴）      |
| `KeywordExtractor.cs`    | 擷取關鍵詞                        |
| `Program.cs`             | 主選單入口                        |
| `appsettings.json`       | 放置 OpenAI 金鑰（**勿上傳**）     |

---

## 🧠 技術堆疊
- C# (.NET 6 / 7)
- OpenAI GPT 模型（Chat API）
- Whisper 語音辨識 API
- Console 應用程式設計

---

## 📌 注意事項
- 請勿將 `appsettings.json` 上傳至 GitHub
- 請使用 `.gitignore` 忽略 `bin/`, `obj/`, `.vs/` 等
