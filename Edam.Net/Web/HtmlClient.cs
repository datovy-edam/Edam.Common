using Edam.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Xml.Linq;

// -----------------------------------------------------------------------------

namespace Edam.Net.Web;

/// <summary>
/// HTML Client...
/// </summary>
public class HtmlClient
{

   /// <summary>
   /// Get HTML Async and optional id of a section within...
   /// </summary>
   /// <param name="url">URL to get</param>
   /// <param name="id">id of section to extract</param>
   /// <returns>Extracted HTML is returned</returns>
   public static async Task<ResultsLog<string>> GetHtmlAsync(
      string url, string id = null)
   {
      ResultsLog<string> results = new ResultsLog<string>();
      using (HttpClient client = new HttpClient())
      {
         try
         {
            results.Instance = await client.GetStringAsync(url);
            if (!String.IsNullOrWhiteSpace(id))
            {
               HtmlDocument doc = new HtmlDocument();
               doc.LoadHtml(results.Instance);
               results.Instance = doc.GetElementbyId(id).OuterHtml;
            }
            results.Succeeded();
         }
         catch (HttpRequestException e)
         {
            results.Failed(e.Message);
         }
      }
      return results;
   }

   /// <summary>
   /// Get HTML and optional id of a section within...
   /// </summary>
   /// <param name="url">URL to get</param>
   /// <param name="id">id of section to extract</param>
   /// <returns>Extracted HTML is returned</returns>
   public static ResultsLog<string> GetHtml(string url, string id = null)
   {
      ResultsLog<string> results = null;
      var tinfo = GetHtmlAsync(url, id);
      tinfo.Wait();
      if (tinfo.Status == TaskStatus.RanToCompletion)
      {
         results = tinfo.Result;
      }
      return results;
   }

}
