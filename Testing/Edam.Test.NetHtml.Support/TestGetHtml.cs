using Edam.Diagnostics;
using Edam.Net.Web;

namespace Edam.Test.NetHtml.Support;

[TestClass]
public sealed class TestGetHtml
{

   [TestMethod]
   public void TestMethod1()
   {
      string url = "https://projecttracking.technology.ca.gov/PAL";
      string id = "projectlist";
      ResultsLog<string> results = HtmlClient.GetHtml(url, id);
   }

}
