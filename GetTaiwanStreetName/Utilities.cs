using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GetTaiwanStreetName
{
    public class Utilities
    {
        private const string city_url = "http://www.post.gov.tw/post/internet/Postal/index.jsp?ID=208";
        private const string address_url = "http://www.post.gov.tw/post/internet/Postal/streetNameData.jsp";

        protected internal static async Task<string> GetHtml(string url)
        {
            string html = string.Empty;
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
                html = await httpClient.GetStringAsync(url);
            }
            catch (Exception ex)
            {
                html = ex.ToString();
            }
            return html;
        }

        protected internal static DataTable GetCityarea()
        {
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load(city_url);
            var script = doc.DocumentNode.SelectSingleNode("//script[contains(.,'cityarea_account = new Array()')]");
            var scriptLines = script.InnerText.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            DataTable cityareaDT = new DataTable();
            cityareaDT.Columns.Add("CityId", typeof(Int32));
            cityareaDT.Columns.Add("Cityarea", typeof(String));

            DataRow dr;
            int CityId = 0;
            string pattern1 = "^cityarea_account\\[(?<cityCode>\\d+)\\] = \\d+;$";
            string pattern2 = "^cityarea\\[\\d+\\] = \\'(?<areaname>.*?)\\';$";
            foreach (var scriptLine in scriptLines)
            {
                if (Regex.IsMatch(scriptLine, pattern1))
                {
                    var group = Regex.Match(scriptLine, pattern1).Groups;
                    CityId = int.Parse(group["cityCode"].Value);
                }
                else
                {
                    if(Regex.IsMatch(scriptLine, pattern2))
                    {
                        var group = Regex.Match(scriptLine, pattern2).Groups;
                        var an = group["areaname"].Value;

                        dr = cityareaDT.NewRow();
                        dr.SetField<int>("CityId", CityId);
                        dr.SetField<string>("Cityarea", an);
                        cityareaDT.Rows.Add(dr);
                    }
                }
            }
            return cityareaDT;
        }

        protected internal static DataTable GetCity()
        {
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load(city_url);
            var options = doc.DocumentNode.SelectNodes("//select[@id='city']/option");

            DataTable cityDT = new DataTable();
            cityDT.Columns.Add("Id", typeof(Int32));
            cityDT.Columns.Add("City", typeof(String));

            int id = 0;
            DataRow dr;
            foreach (var node in options)
            {
                string city = node.Attributes["Value"].Value;
                if (city == "%")
                    continue;

                dr = cityDT.NewRow();
                dr.SetField<int>("Id", id++);
                dr.SetField<string>("City", node.Attributes["Value"].Value);
                cityDT.Rows.Add(dr);
            }
            return cityDT;
        }

        protected internal async static Task<string> GetAddress(string city, string cityarea)
        {
            HttpClient clint = new HttpClient();
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("city", city));
            postData.Add(new KeyValuePair<string, string>("cityarea", cityarea));
            HttpContent content = new FormUrlEncodedContent(postData);
            HttpResponseMessage message = await clint.PostAsync(address_url, content);
            return await message.Content.ReadAsStringAsync();
        }

        protected internal static List<string> ParserAddress(string addr)
        {
            XElement root = XElement.Parse(addr, LoadOptions.PreserveWhitespace);
            return root.Descendants("array0").Select(x => x.Value).ToList();
        }
    }
}
