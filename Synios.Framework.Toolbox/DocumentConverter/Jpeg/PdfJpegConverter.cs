using System;
using System.IO;
using System.Collections.Generic;
using Synios.Framework.Toolbox.DocumentConverter.Interfaces;
using Synios.Framework.Toolbox.Registration;
using Vintasoft.Imaging;
using Vintasoft.Imaging.Codecs.Encoders;
using Vintasoft.Imaging.Pdf;

namespace Synios.Framework.Toolbox.DocumentConverter.Jpeg
{
    public class PdfJpegConverter : IConverter
    {
        private const string Jpeg = ".jpg";

        public PdfJpegConverter()
        {
            try
            {
                SingletonVintasoftImagingRegister.GetInstance().Register();
            }
            catch (Exception ex)
            {
                FileLogger.FileLogger.Instance.WriteExeption(ex);
            }
        }

        //public bool Colour { get; set; }
        //public int Compression { get; set; }
        //public bool KeepOriginal { get; set; }
       
        #region Jpeg

        /// <summary>
        /// Von dem PDF-Dokument wird die erste Seite in Jpeg konvertiert und gespeichert.
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

                using (PdfDocument pdfDocument = new PdfDocument(param.FilePath, true))
                {
                    pdfDocument.RenderingSettings.Resolution = new Resolution(150, 150);
                    JpegEncoder jpeGencoder = new JpegEncoder
                    {
                        Settings = { Quality = param.Quality, SaveAsGrayscale = false }
                    };


                    for (int pageCount = 0; pageCount < 1; pageCount++)
                    {
                        string tempName = FilesFunctions.GetRandomName(pathFolder, fileInfo, true, Jpeg);
                        // get image of PDF page as VintasoftImage object
                        using (VintasoftImage vsImage = pdfDocument.Pages[pageCount].Render())
                        {
                            // save VintasoftImage object as JPEG file
                            vsImage.Save(tempName, jpeGencoder);
                            fileName = tempName;
                        }
                    }
                }

                return fileName;
            }
            catch (Exception ex)
            {
                FileLogger.FileLogger.Instance.WriteExeption(ex);
                return string.Empty;
            }

        }

        /// <summary>
        /// Konvertiert die PDF-Datei in Jpeg-Dateien.
        /// Gibt eine liste mit den Pfaden der Jpeg-Dateien zurück.
        /// </summary>
        public List<string> ConvertPDFToImage(string pathFolder, bool tempPrefix, DocumentParam param)
        {
            List<string> list = new List<string>();
            try
            {

                if (string.CompareOrdinal(pathFolder, string.Empty) == 0)
                    return list;

                FileInfo fileInfo = new FileInfo(param.FilePath);

                if (fileInfo.Exists == false)
                    return list;



                FilesFunctions.DeleteFilesInFolder(pathFolder, Jpeg);

                using (PdfDocument pdfDocument = new PdfDocument(param.FilePath, true))
                {
                    //pdfDocument.RenderingSettings.Resolution = new Resolution(150, 150);
                    JpegEncoder jpeGencoder = new JpegEncoder
                    {
                        Settings = {Quality = param.Quality, SaveAsGrayscale = false}
                    };


                    for (int pageCount = 0; pageCount < pdfDocument.Pages.Count; pageCount++)
                    {
                        string tempName = FilesFunctions.GetRandomName(pathFolder, fileInfo, tempPrefix, Jpeg);
                        // get image of PDF page as VintasoftImage object
                        using (VintasoftImage vsImage = pdfDocument.Pages[pageCount].Render())
                        {
                            // save VintasoftImage object as JPEG file
                            vsImage.Save(tempName, jpeGencoder);
                            list.Add(tempName);
                        }
                    }
                }
                return list;

            }
            catch (Exception ex)
            {
                FileLogger.FileLogger.Instance.WriteExeption(ex);
                return list;
            }
        }

        /// <summary>
        /// Konvertiert die PDF-Datei in Jpeg-Dateien.
        /// Die Dateien werden in den Temp-Ordner des aktuellen Benutzers erzeugt.
        /// Gibt eine liste mit den Pfaden der Jpeg-Dateien zurück.
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
        #endregion
    }
}
