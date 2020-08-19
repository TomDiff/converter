using System.IO;
using System.Collections.Generic;
using System;
using Synios.Framework.Toolbox.DocumentConverter.Interfaces;
using Synios.Framework.Toolbox.DocumentConverter.Jpeg;
using Synios.Framework.Toolbox.DocumentConverter.Tiff;
using Synios.Framework.Toolbox.DocumentConverter.Enums;

namespace Synios.Framework.Toolbox.DocumentConverter
{

    public class ConvertDocument : IDocumentConverter
    {
        private readonly Dictionary<eToConvertTyp, IConverter> _pdfConverters = new Dictionary<eToConvertTyp, IConverter>();
        private readonly Dictionary<eToConvertTyp, IConverter> _docConverters = new Dictionary<eToConvertTyp, IConverter>();
        public ConvertDocument()
        {
            _pdfConverters.Add(eToConvertTyp.Jpg, new PdfJpegConverter());
            _pdfConverters.Add(eToConvertTyp.Tif, new PdfTiffConverter());
            _docConverters.Add(eToConvertTyp.Tif, new DocxTiffConverter());
            _docConverters.Add(eToConvertTyp.Jpg, new DocxJpegConverter());
        }
        public string GetFirstPageAsImageFromDocument(DocumentParam param)
        {
            string outFile = string.Empty;
            if (param.Doctyp == eDocumentTyp.PDF)
                outFile = _pdfConverters[param.ConTyp].GetFirstPageAsImageFromDocument(param);
            else if (param.Doctyp == eDocumentTyp.WORD)
                outFile = _docConverters[param.ConTyp].GetFirstPageAsImageFromDocument(param);

            return outFile;
        }
        public List<string> ConvertDocumentToImage(DocumentParam param)
        {
            List<string> outList = new List<string>();
            if (param.Doctyp == eDocumentTyp.PDF)
                outList = _pdfConverters[param.ConTyp].ConvertPDFToImage(param);
            else if (param.Doctyp == eDocumentTyp.WORD)
                outList = _docConverters[param.ConTyp].ConvertPDFToImage(param);
            return outList;
        }
        public List<string> ConvertToImage(DocumentParam param)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(param.FilePath);

                List<string> outList = new List<string>();
                if (param.Doctyp == eDocumentTyp.PDF)
                    outList = _pdfConverters[param.ConTyp].ConvertPDFToImage(fileInfo.DirectoryName, false, param);
                else if (param.Doctyp == eDocumentTyp.WORD)
                    outList = _docConverters[param.ConTyp].ConvertPDFToImage(fileInfo.DirectoryName, false, param);

                return outList;
            }
            catch (Exception ex)
            {
                FileLogger.FileLogger.Instance.WriteExeption(ex);
                return new List<string>();
            }
        }

    }// Class end
}
