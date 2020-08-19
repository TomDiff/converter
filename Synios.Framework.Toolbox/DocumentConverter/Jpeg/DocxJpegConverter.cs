using System;
using System.IO;
using System.Collections.Generic;
using Synios.Framework.Toolbox.DocumentConverter.Interfaces;
using DevExpress.XtraRichEdit;
using DevExpress.XtraPrinting;

namespace Synios.Framework.Toolbox.DocumentConverter.Jpeg
{
    public class DocxJpegConverter : IConverter
    {
        private const string Jpeg = ".jpg";
        private const string Pdf = ".pdf";

        public DocxJpegConverter()
        {
            try
            {
                //SingletonVintasoftImagingRegister.GetInstance().Register();
            }
            catch (Exception ex)
            {
                FileLogger.FileLogger.Instance.WriteExeption(ex);
            }
        }

        /// <summary>
        /// Konvertiert die Doc-Datei in Tiff-Dateien.
        /// Gibt eine liste mit den Pfaden der Tiff-Dateien zurück.
        /// </summary>
        public List<string> ConvertPDFToImage(string pathFolder, bool tempPrefix, DocumentParam param)
        {
            List<string> list = new List<string>();
            try
            {
                FileInfo fileInfo = new FileInfo(param.FilePath);

                if (fileInfo.Exists == false)
                    return list;

                FilesFunctions.DeleteFilesInFolder(pathFolder, Jpeg);
                //string tempName = FilesFunctions.GetRandomName(pathFolder, fileInfo, tempPrefix, Pdf);

                string temp = $@"{Path.GetTempPath()}\Synios\{Path.GetFileNameWithoutExtension(fileInfo.FullName)}{Pdf}";

                using (RichEditDocumentServer richServer = new RichEditDocumentServer())
                {
                    richServer.LoadDocument(param.FilePath);

                    //Specify export options:
                    PdfExportOptions options = new PdfExportOptions
                    {
                        Compressed = false, ImageQuality = PdfJpegImageQuality.Highest
                    };
                    //Export the document to the stream: 
                   
                    using (FileStream pdfFileStream = new FileStream(temp, FileMode.Create))
                    {
                        richServer.ExportToPdf(pdfFileStream, options);
                    }
                }


                var pdf = new  PdfJpegConverter();

                var parammeter = new DocumentParam
                {
                    Resolution = param.Resolution,
                    Colour = param.Colour,
                    Compression = param.Compression,
                    Quality = param.Quality,
                    KeepOriginal = param.KeepOriginal,
                    ConTyp = param.ConTyp,
                    Doctyp = param.Doctyp,
                    FilePath = temp
                };

                list = pdf.ConvertPDFToImage(pathFolder, tempPrefix, parammeter);


                return list;
            }
            catch (Exception ex)
            {
                FileLogger.FileLogger.Instance.WriteExeption(ex);
                return list;
            }
        }

        /// <summary>
        /// Konvertiert die Doc-Datei in Tiff-Dateien.
        /// Die Dateien werden in den Temp-Ordner des aktuellen Benutzers erzeugt.
        /// Gibt eine liste mit den Pfaden der Tiff-Dateien zurück.
        /// </summary>
        public List<string> ConvertPDFToImage(DocumentParam param)
        {
            List<string> list = new List<string>();
            try
            {
                FileInfo fileInfo = new FileInfo(param.FilePath);

                if (fileInfo.Exists == false)
                    return list;

                string pathFolder = FilesFunctions.CreateFolder(fileInfo);
                if (string.CompareOrdinal(pathFolder, string.Empty) == 0)
                    return list;

                list = ConvertPDFToImage(pathFolder, true, param);

                return list;
            }
            catch (Exception ex)
            {
                FileLogger.FileLogger.Instance.WriteExeption(ex);
                return list;
            }
        }


        /// <summary>
        /// Von dem Doc-Dokument wird die erste Seite in tiff konvertiert und gespeichert.
        /// die funktion liefert den Path zu der neue Datei zurück.
        /// </summary>
        public string GetFirstPageAsImageFromDocument(DocumentParam param)
        {
            try
            {
                string fileName = string.Empty;
                FileInfo fileInfo = new FileInfo(param.FilePath);

                if (fileInfo.Exists == false)
                    return fileName;

                string pathFolder = Path.GetTempPath();

                FilesFunctions.DeleteFilesInFolder(pathFolder, Jpeg);
                string tempName = FilesFunctions.GetRandomName(pathFolder, fileInfo, true, Pdf);
                using (RichEditDocumentServer richServer = new RichEditDocumentServer())
                {
                    richServer.LoadDocument(param.FilePath);

                    //Specify export options:
                    PdfExportOptions options = new PdfExportOptions
                    {
                        Compressed = false,
                        ImageQuality = PdfJpegImageQuality.Highest
                    };
                    //Export the document to the stream: 

                    using (FileStream pdfFileStream = new FileStream(tempName, FileMode.Create))
                    {
                        richServer.ExportToPdf(pdfFileStream, options);
                    }
                }

                var pdf = new PdfJpegConverter();

                var parammeter = new DocumentParam
                {
                    Resolution = param.Resolution,
                    Colour = param.Colour,
                    Compression = param.Compression,
                    Quality = param.Quality,
                    KeepOriginal = param.KeepOriginal,
                    ConTyp = param.ConTyp,
                    Doctyp = param.Doctyp,
                    FilePath = tempName
                };

                fileName = pdf.GetFirstPageAsImageFromDocument(parammeter);

                return fileName;
            }
            catch (Exception ex)
            {
                FileLogger.FileLogger.Instance.WriteExeption(ex);
                return string.Empty;
            }
        }
    }
}
