# GetTaiwanStreetName

GetTaiwanStreetName is a Windows Forms application designed to fetch, display, and export street names in Taiwan. Users can navigate through cities and their respective areas to find specific street information.

## How to Use

The application interface is organized into tabs for different functionalities:

*   **View Cities (Tab 1):** Displays a list of cities.
*   **View City Areas (Tab 2):** Allows you to select a city from a dropdown menu, then displays the areas within that city.
*   **Find Streets (Tab 3):** Allows you to select a city and a specific city area from dropdown menus. The application will then display the street names within the selected area.

### Exporting Data

The application allows you to export all the street data into a JSON file.
- Click the "輸出資料" (Export Data) button.
- A folder browser dialog will appear. Select the desired location where you want to save the `taiwan.json` file.
- The application will fetch all city, city area, and street data and save it as `taiwan.json` in the chosen directory.

## Data Source

The application retrieves data from Taiwan's official postal service website:

*   **City and City Area Data:** Scraped from [http://www.post.gov.tw/post/internet/Postal/index.jsp?ID=208](http://www.post.gov.tw/post/internet/Postal/index.jsp?ID=208)
*   **Street Name Data:** Fetched from [http://www.post.gov.tw/post/internet/Postal/streetNameData.jsp](http://www.post.gov.tw/post/internet/Postal/streetNameData.jsp) via a POST request.

**Note:** The reliability of the data depends on the availability and structure of these external web pages.

## Building and Running the Project

### Prerequisites
*   Microsoft Visual Studio (Tested with Visual Studio 2022, but older versions compatible with .NET Framework 4.7.2 or later should work)
*   .NET Framework 4.7.2 (or a later compatible version)

### Steps
1.  **Clone the repository:**
    ```bash
    git clone <repository_url>
    ```
    (Replace `<repository_url>` with the actual URL of this repository)
2.  **Open the solution:**
    Open the `Max.sln` file in Visual Studio.
3.  **Restore NuGet Packages:**
    Visual Studio should automatically restore the NuGet packages listed in `packages.config` (e.g., Newtonsoft.Json, HtmlAgilityPack). If not, you might need to do it manually through the NuGet Package Manager:
    *   Right-click on the solution in Solution Explorer.
    *   Select "Manage NuGet Packages for Solution..."
    *   Go to the "Restore" or "Installed" tab and ensure all packages are restored/installed.
4.  **Build the solution:**
    *   From the Visual Studio menu, select "Build" > "Build Solution" (or press `Ctrl+Shift+B`).
5.  **Run the application:**
    *   From the Visual Studio menu, select "Debug" > "Start Debugging" (or press `F5`).
    *   Alternatively, you can find the compiled executable in the `GetTaiwanStreetName/bin/Debug` (or `GetTaiwanStreetName/bin/Release`) folder and run it directly.

### Dependencies
The project relies on the following NuGet packages:
*   **Newtonsoft.Json:** For working with JSON data (used in exporting).
*   **HtmlAgilityPack:** For parsing HTML content (used in web scraping).
