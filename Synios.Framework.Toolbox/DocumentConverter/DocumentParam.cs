using Synios.Framework.Toolbox.DocumentConverter.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Synios.Framework.Toolbox.DocumentConverter
{
    public class DocumentParam
    {
        public bool Colour { get; set; }
        public int Compression { get; set; }
        public int Resolution { get; set; }
        public int Quality { get; set; }
        public bool KeepOriginal { get; set; }
        public string FilePath { get; set; }
        public eToConvertTyp ConTyp { get; set; }
        public eDocumentTyp Doctyp { get; set; }

        public DocumentParam()
        {
            Colour = true;
            Compression = 100;
            Resolution = 150;
            Quality = 80;
            KeepOriginal = true;
            FilePath = "";
            ConTyp = eToConvertTyp.Jpg;
            Doctyp = eDocumentTyp.PDF;
        }
    }
}
