using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

// -----------------------------------------------------------------------------

namespace Edam.Net.Web;

public class HtmlTableInfo
{
   public Dictionary<string, string> HeaderMap { get; set; } = 
      new Dictionary<string, string>();

   private List<string> _header { get; set; }
   public List<string> Header
   {
      get { return _header; }
   }
   public List<List<HtmlCellInfo>> Rows { get; set; } = 
      new List<List<HtmlCellInfo>>();

   /// <summary>
   /// Add Row (cells).
   /// </summary>
   /// <param name="row">list of cells</param>
   public void AddRow(List<HtmlCellInfo> row)
   {
      Rows.Add(row);
   }

   /// <summary>
   /// Get Header.
   /// </summary>
   /// <param name="header"></param>
   public void SetHeader(string header)
   {
      if (_header == null)
      {
         _header = new List<string>();
      }
      _header.Clear();
      var list = header.Split("\r\n");
      foreach (var item in list)
      {
         if (String.IsNullOrWhiteSpace(item))
            continue;
         _header.Add(item.Trim());
      }
   }

   /// <summary>
   /// Map a key to a value within the dictionary.
   /// </summary>
   /// <param name="key">key to search</param>
   /// <returns>found value is returned</returns>
   private string Map(string key)
   {
      if (!HeaderMap.TryGetValue(key, out string value))
      {
         value = key;
      }
      return value;
   }

   /// <summary>
   /// Prpeare Json
   /// </summary>
   /// <param name="key"></param>
   /// <param name="value"></param>
   /// <param name="close"></param>
   /// <returns></returns>
   private string PrepareJson(string key, string value, bool close = false)
   {
      return "      \"" + key + "\": \"" + value + "\"" +
         (close ? "," : String.Empty);
   }

   /// <summary>
   /// Prepare JSON document representing this table.
   /// </summary>
   /// <returns>JSON Text</returns>
   public string ToJsonDocument()
   {
      StringBuilder sb = new StringBuilder();
      int cnt = 0;
      sb.AppendLine("[");
      foreach (var row in Rows)
      {
         var indx = 0;
         if (cnt != 0)
            sb.AppendLine(",");
         sb.AppendLine("   {");
         foreach (var cell in row)
         {

            // there must be more than 1 attribute
            if (cell.AttributeCount <= 1)
               continue;

            if (indx != 0)
               sb.AppendLine(",");

            var header = _header[indx];
            if (!String.IsNullOrWhiteSpace(cell.Title))
            {
               sb.AppendLine(PrepareJson(Map(nameof(cell.Title)), cell.Title, true));
            }
            if (!String.IsNullOrWhiteSpace(cell.Link))
            {
               sb.AppendLine(PrepareJson(Map(nameof(cell.Link)), cell.Link, true));
            }
            if (!String.IsNullOrWhiteSpace(cell.Description))
            {
               sb.AppendLine(PrepareJson(Map(nameof(cell.Description)), cell.Description, true));
            }

            sb.Append(PrepareJson(Map(header), cell.Text));
            indx++;
         }
         sb.AppendLine(String.Empty);
         sb.Append("   }");
         cnt++;
      }
      sb.AppendLine(String.Empty);
      sb.AppendLine("]");
      return sb.ToString();
   }

}
