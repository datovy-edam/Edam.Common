using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// -----------------------------------------------------------------------------

namespace Edam.Diagnostics;

/// <summary>
/// Support tracking time - tracking elapse of time.
/// </summary>
public class LogTimingInfo
{
   public const string TIME_ELAPSE = "Elap";
   private DateTime _startTime;

   public LogTimingInfo()
   {
      _startTime = DateTime.Now;
   }

   /// <summary>
   /// Reset Start Date - Time.
   /// </summary>
   public void Reset()
   {
      _startTime = DateTime.Now;
   }

   /// <summary>
   /// Trace Timming as of now with given (optional) message.
   /// </summary>
   /// <param name="header"></param>
   /// <returns></returns>
   public string ToString(string header = null)
   {
      DateTime now = DateTime.Now;
      TimeSpan delta = now - _startTime;
      string mess = String.IsNullOrWhiteSpace(header) ? 
         TIME_ELAPSE : header;
      var formatted = delta.ToString(@"hh\:mm\:ss\.fff");
      mess += String.Format("{0} ({1} ms)", mess, formatted);
      return mess;
   }

}
