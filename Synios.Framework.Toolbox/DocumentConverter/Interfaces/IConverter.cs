using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Synios.Framework.Toolbox.DocumentConverter.Interfaces
{
    public interface IConverter
    {
        string GetFirstPageAsImageFromDocument(DocumentParam param);
        List<string> ConvertPDFToImage(string PathFolder, bool TempPrefix, DocumentParam param);
        List<string> ConvertPDFToImage(DocumentParam param);
    }
}
