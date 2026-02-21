# GetTaiwanStreetName

GetTaiwanStreetName 是一個 Windows Forms 應用程式，用於擷取、顯示並匯出台灣的街道名稱。使用者可以瀏覽各縣市及其下轄區域，以查詢特定的街道資訊。

## 使用方式

應用程式介面以分頁（Tab）方式組織不同功能：

*   **檢視縣市（第 1 頁）：** 顯示縣市清單。
*   **檢視行政區（第 2 頁）：** 從下拉選單選擇縣市後，顯示該縣市下的行政區。
*   **查詢街道（第 3 頁）：** 從下拉選單選擇縣市與行政區後，顯示該區域內的街道名稱。

### 匯出資料

本應用程式可將所有街道資料匯出為 JSON 檔案。
- 點擊「輸出資料」按鈕。
- 出現資料夾瀏覽對話框後，選擇欲儲存 `taiwan.json` 的目標位置。
- 應用程式將擷取所有縣市、行政區及街道資料，並以 `taiwan.json` 的名稱儲存至指定目錄。

## 資料來源

本應用程式從台灣官方郵政服務網站擷取資料：

*   **縣市與行政區資料：** 抓取自 [http://www.post.gov.tw/post/internet/Postal/index.jsp?ID=208](http://www.post.gov.tw/post/internet/Postal/index.jsp?ID=208)
*   **街道名稱資料：** 透過 POST 請求從 [http://www.post.gov.tw/post/internet/Postal/streetNameData.jsp](http://www.post.gov.tw/post/internet/Postal/streetNameData.jsp) 取得。

**注意：** 資料的正確性取決於上述外部網頁的可用性與結構。

## 建置與執行專案

### 事前準備
*   Microsoft Visual Studio（已於 Visual Studio 2022 測試，支援 .NET Framework 4.7.2 或更新版本的舊版本亦可使用）
*   .NET Framework 4.7.2（或更新的相容版本）

### 步驟
1.  **複製儲存庫：**
    ```bash
    git clone <repository_url>
    ```
    （將 `<repository_url>` 替換為本儲存庫的實際 URL）
2.  **開啟方案：**
    以 Visual Studio 開啟 `Max.sln` 檔案。
3.  **還原 NuGet 套件：**
    Visual Studio 通常會自動還原 `packages.config` 中列出的 NuGet 套件（如 Newtonsoft.Json、HtmlAgilityPack）。若未自動還原，可透過 NuGet 套件管理員手動執行：
    *   在方案總管中，對方案按右鍵。
    *   選擇「管理方案的 NuGet 套件...」。
    *   前往「還原」或「已安裝」頁籤，確認所有套件皆已還原/安裝。
4.  **建置方案：**
    *   從 Visual Studio 選單選擇「建置」>「建置方案」（或按 `Ctrl+Shift+B`）。
5.  **執行應用程式：**
    *   從 Visual Studio 選單選擇「偵錯」>「開始偵錯」（或按 `F5`）。
    *   或者，也可在 `GetTaiwanStreetName/bin/Debug`（或 `GetTaiwanStreetName/bin/Release`）資料夾中找到編譯後的執行檔並直接執行。

### 相依套件
本專案依賴以下 NuGet 套件：
*   **Newtonsoft.Json：** 用於處理 JSON 資料（匯出功能使用）。
*   **HtmlAgilityPack：** 用於解析 HTML 內容（網頁抓取使用）。
