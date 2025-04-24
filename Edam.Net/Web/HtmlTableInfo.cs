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
   /// Cell to JSON.
   /// </summary>
   /// <param name="builder">builder</param>
   /// <param name="cell">cell to process</param>
   /// <returns></returns>
   private StringBuilder CellToJson(StringBuilder builder, HtmlCellInfo cell)
   {
      if (cell.OrtinalNo != 0)
         builder.AppendLine(",");

      var header = _header[cell.OrtinalNo];
      if (!String.IsNullOrWhiteSpace(cell.Title))
      {
         builder.AppendLine(PrepareJson(Map(nameof(cell.Title)), cell.Title, true));
      }
      if (!String.IsNullOrWhiteSpace(cell.Link))
      {
         builder.AppendLine(PrepareJson(Map(nameof(cell.Link)), cell.Link, true));
      }
      if (!String.IsNullOrWhiteSpace(cell.Description))
      {
         builder.AppendLine(PrepareJson(
            Map(nameof(cell.Description)), cell.Description, true));
      }

      builder.Append(PrepareJson(Map(header), cell.Text));

      return builder;
   }

   /// <summary>
   /// Prepare JSON document representing this table.
   /// </summary>
   /// <param name="evaluateRow">function to evaluate row that returns true or false</param>
   /// <returns>JSON Text</returns>
   public string ToJsonDocument(Func<List<HtmlCellInfo>,bool> evaluateRow = null)
   {
      StringBuilder sb = new StringBuilder();
      int cnt = 0;
      sb.AppendLine("[");
      foreach (var row in Rows)
      {

         // prepare row cells JSON
         StringBuilder? cellBuilder = new StringBuilder();
         var cellIndex = 0;
         foreach (var cell in row)
         {
            CellToJson(cellBuilder, cell);
            cellIndex++;
         }

         if (cellIndex > 1 && cellBuilder != null)
         {
            var include = true;
            if (evaluateRow != null)
            {
               include = evaluateRow(row);
            }
            if (include)
            {
               if (cnt != 0)
                  sb.AppendLine(",");
               sb.AppendLine("   {");
               sb.Append(cellBuilder);
               sb.AppendLine(String.Empty);
               sb.Append("   }");
               cnt++;
            }
         }
      }

      sb.AppendLine(String.Empty);
      sb.AppendLine("]");
      return sb.ToString();
   }

}
