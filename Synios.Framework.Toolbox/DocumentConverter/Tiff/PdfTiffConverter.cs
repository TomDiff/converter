using System;
using System.IO;
using System.Collections.Generic;
using Synios.Framework.Toolbox.DocumentConverter.Interfaces;
using Synios.Framework.Toolbox.Registration;
using Vintasoft.Imaging;
using Vintasoft.Imaging.Codecs.ImageFiles.Tiff;
using Vintasoft.Imaging.Pdf;

namespace Synios.Framework.Toolbox.DocumentConverter.Tiff
{
    internal class PdfTiffConverter : IConverter
    {

        private const string Tiff = ".tif";

        public PdfTiffConverter()
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

        /// <summary>
        /// Von dem PDF-Dokument wird die erste Seite in tiff konvertiert und gespeichert.
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
                    // set resolution
                    pdfDocument.RenderingSettings.Resolution = new Resolution(param.Resolution, param.Resolution);

                    // set rendering mode - optimal balance between rendering speed and quality
                    pdfDocument.RenderingSettings.RenderingMode = PdfRenderingMode.HighQuality;

                    string tempName = FilesFunctions.GetRandomName(pathFolder, fileInfo, true, Tiff);
                    for (int pageCount = 0; pageCount < pdfDocument.Pages.Count; pageCount++)
                    {
                       

                        // create new TIFF file
                        using (TiffFile tiffFile = new TiffFile(tempName, TiffFileFormat.LittleEndian))
                        {
                            tiffFile.Pages.EncoderSettings.Compression = TiffCompression.Lzw;
                            tiffFile.Pages.Add(pdfDocument.Pages[pageCount].Render());
                            tiffFile.SaveChanges();
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
        /// Konvertiert die PDF-Datei in Tiff-Dateien.
        /// Gibt eine liste mit den Pfaden der Tiff-Dateien zurück.
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
               
                FilesFunctions.DeleteFilesInFolder(pathFolder,Tiff);

                // open existing PDF document
                using (PdfDocument pdfDocument = new PdfDocument(param.FilePath, true))
                {
                    // set resolution
                    //pdfDocument.RenderingSettings.Resolution = new Resolution(param.Resolution, param.Resolution);

                    //// set rendering mode - optimal balance between rendering speed and quality
                    //pdfDocument.RenderingSettings.RenderingMode = PdfRenderingMode.HighSpeed;

                    for (int pageCount = 0; pageCount < pdfDocument.Pages.Count; pageCount++)
                    {
                        string tempName = FilesFunctions.GetRandomName(pathFolder, fileInfo, tempPrefix, Tiff);
     
                        // create new TIFF file
                        using (TiffFile tiffFile = new TiffFile(tempName, TiffFileFormat.LittleEndian))
                        {
                            tiffFile.Pages.EncoderSettings.Compression = TiffCompression.Zip;
                            tiffFile.Pages.Add(pdfDocument.Pages[pageCount].Render());
                            tiffFile.SaveChanges();
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
        /// Konvertiert die PDF-Datei in Tiff-Dateien.
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

    } //class End
}
