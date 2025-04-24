using Edam.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Xml.Linq;
using System.Dynamic;

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
   /// Scrap Attributes
   /// </summary>
   /// <param name="node">node whose attributes will be scrapped</param>
   /// <param name="cell">cell info to store attributes into</param>
   public static void ScrapAttributes(
      HtmlNode node, HtmlCellInfo cell)
   {
      string title = String.Empty;
      string link = String.Empty;
      foreach (var n in node.Attributes)
      {
         if (n.Name == "title")
         {
            title = n.Value;
         }
         else if (n.Name == "href")
         {
            link = n.Value;
         }
      }

      if (!String.IsNullOrWhiteSpace(link))
      {
         cell.Link = link;
         cell.Description = title;
      }
      else
      {
         cell.Title = title;
      }
   }

   /// <summary>
   /// Scrap Link.
   /// </summary>
   /// <param name="node">node whose child nodes will be scrapped</param>
   /// <param name="cell">cell info to store attributes into</param>
   public static void ScrapLink(
      HtmlNode node, HtmlCellInfo cell)
   {
      if (node.InnerHtml.StartsWith("<a "))
      {
         foreach(var c in node.ChildNodes)
         {
            if (c.HasAttributes)
            {
               ScrapAttributes(c, cell);
            }
         }
      }
      else
      {
         ScrapAttributes(node, cell);
      }
   }

   /// <summary>
   /// Get HTML Async and optional id of a section within...
   /// </summary>
   /// <param name="url">URL to get</param>
   /// <param name="id">id of section to extract</param>
   /// <returns>Extracted HTML is returned</returns>
   public static async Task<ResultsLog<HtmlTableInfo>> GetHtmlTableAsync(
      string url, string id = null)
   {
      ResultsLog<HtmlTableInfo> results = new ResultsLog<HtmlTableInfo>();
      results.Instance = new HtmlTableInfo();
      HtmlTableInfo tinfo = results.Instance;

      var web = new HtmlWeb();
      var doc = await web.LoadFromWebAsync(url);

      // Select the first table
      var table = doc.DocumentNode.SelectSingleNode("//table");
      if (table != null)
      {
         var rows = table.SelectNodes(".//tr");
         foreach (var row in rows)
         {
            var cells = row.SelectNodes(".//td");
            if (cells != null)
            {
               int cnt = 0;
               var items = new List<HtmlCellInfo>();
               foreach (var cell in cells)
               {
                  var cellInfo = new HtmlCellInfo();
                  cellInfo.OrtinalNo = cnt;

                  // check inner cell, it is a Text cell?
                  if (cell.InnerText != String.Empty)
                  {
                     if (String.IsNullOrWhiteSpace(cell.InnerHtml))
                        continue;

                     ScrapLink(cell, cellInfo);
                     cellInfo.Text = cell.InnerText.Trim();
                     items.Add(cellInfo);
                  }
                  else
                  {

                  }
                  cnt++;
               }
               if (items.Count > 0)
                  tinfo.AddRow(items);
            }
            else
            {
               // assume that first line is <th> with header info
               tinfo.SetHeader(row.InnerText);
            }
         }
      }
      else
      {
         Console.WriteLine("No table found.");
      }
      return results;
   }

   /// <summary>
   /// Get HTML and optional id of a section within...
   /// </summary>
   /// <param name="url">URL to get</param>
   /// <returns>Extracted HTML is returned</returns>
   public static ResultsLog<HtmlTableInfo> GetHtmlTable(string url)
   {
      ResultsLog<HtmlTableInfo> results = null;
      var tinfo = GetHtmlTableAsync(url);
      tinfo.Wait();
      if (tinfo.Status == TaskStatus.RanToCompletion)
      {
         results = tinfo.Result;
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
