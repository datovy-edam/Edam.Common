using Edam.Diagnostics;
using Edam.Net.Web;

namespace Edam.Test.NetHtml.Support;

[TestClass]
public sealed class TestGetHtml
{

   [TestMethod]
   public void TestHtmlScraper()
   {
      string url = "https://projecttracking.technology.ca.gov/PAL";
      string id = "projectlist";
      ResultsLog<string> results = HtmlClient.GetHtml(url, id);
   }

   [TestMethod]
   public void TestHtmlTableScraper()
   {
      Dictionary<string, string> keyValues = new Dictionary<string, string>();
      keyValues.Add("Title", "Agency Name");

      string url = "https://projecttracking.technology.ca.gov/PAL";
      ResultsLog<HtmlTableInfo> results = HtmlClient.GetHtmlTable(url);
      results.Instance.HeaderMap = keyValues;
      string jsonText = results.Instance.ToJsonDocument();
   }

   [TestMethod]
   public void TestTiming()
   {
      LogTimingInfo elap = new LogTimingInfo();
      Task.Delay(1000).Wait();
      string mess = elap.ToString();
   }
}
