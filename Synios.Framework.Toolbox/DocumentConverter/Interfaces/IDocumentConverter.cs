using System;
using System.Collections.Generic;

namespace Synios.Framework.Toolbox.DocumentConverter.Interfaces
{
    public interface IDocumentConverter
    {
        string GetFirstPageAsImageFromDocument(DocumentParam param);
        List<string> ConvertDocumentToImage(DocumentParam param);
        List<string> ConvertToImage(DocumentParam param);
    }
}
