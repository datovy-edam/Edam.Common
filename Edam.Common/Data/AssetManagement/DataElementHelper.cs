using Edam.Data.Asset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace Edam.Data.AssetManagement;

public class DataElementHelper
{
   #region -- 4.0 - Support Methods

   public static string GetUnqualifyText(
      string qualifiedText, string separator = "/",
      string newSeparator = "_")
   {
      string token;
      string unqualifiedText = qualifiedText;
      int indx = 0;
      do
      {
         string[] l = unqualifiedText.Split(separator);
         if (l.Length == 0)
         {
            break;
         }
         token = null;
         foreach (string s in l)
         {
            indx = s.IndexOf(':');
            if (indx != -1)
            {
               token = s;
               break;
            }
         }
         if (token == null)
         {
            break;
         }
         unqualifiedText =
            unqualifiedText.Replace(token.Substring(0, indx + 1), "");
      } while (true);
      return unqualifiedText.Replace(separator, newSeparator);
   }

   #endregion
}
