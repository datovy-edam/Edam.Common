using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// -----------------------------------------------------------------------------

namespace Edam.DataObjects.Medias
{

   public enum MediaFormat
   {
      Unknown = 0,
      JPEG = 1,
      MPEG = 2,
      TextFile = 3,
      RtfFile = 4,
      MsWordFile = 5,
      PdfFile = 6,
      XmlDocument = 7,
      PNG = 8,
      OfficeWordXml = 9,
      OfficeExcelXml = 17,
      XML = 11,
      XSLT = 14,
      JSON = 16,
      SQL = 18,
      Markdown = 19,
      Yaml = 20,

      JPEG2000 = 50,
      WSQ = 51,
      Bitmap = 52,
      VectorDatav = 53,
      FaxGroup4Standard = 54
   }

}
