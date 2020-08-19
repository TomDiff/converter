//using System;
//using System.Collections.Generic;
//using System.Text;

//using Microsoft.Office.Interop.Word;
//using Microsoft.Office.Interop.Excel;



//namespace Synios.Framework.Toolbox.DocumentConverter
//{
//    //Die Klasse verwedet die intallierte Office Programme, um die Dokumenten in PDF zu wandelt.
//    public class OfficeToPdfConverter
//    {
//        public static void Word2Pdf(string wordDocFile, string pdfDocFile)
//        {
//            object falseObj = false;
//            object trueObj = true;
//            object emptyStr = "";
//            object missing = System.Reflection.Missing.Value;

//            Microsoft.Office.Interop.Word.Application wordApp = null;
//            try
//            {
//                wordApp = new Microsoft.Office.Interop.Word.Application();
//                wordApp.Visible = false;
//                object wordDocFileObj = wordDocFile;
//                Microsoft.Office.Interop.Word.Document wordDoc = null;
//                try
//                {
//                    wordDoc = wordApp.Documents.Open(ref wordDocFileObj, ref falseObj, ref trueObj, ref missing, ref missing,
//                        ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);

//                    wordDoc.ExportAsFixedFormat(pdfDocFile, WdExportFormat.wdExportFormatPDF, false, WdExportOptimizeFor.wdExportOptimizeForOnScreen
//                        , WdExportRange.wdExportAllDocument, -1, -1, WdExportItem.wdExportDocumentWithMarkup, true, true, WdExportCreateBookmarks.wdExportCreateNoBookmarks,
//                        false, true, true, ref missing);

//                }
//                finally
//                {
//                    if (wordDoc != null)
//                        wordDoc.Close(ref falseObj, ref missing, ref missing);
//                    wordDoc = null;
//                }
//            }
//            finally
//            {
//                if (wordApp != null)
//                    wordApp.Quit(ref falseObj, ref missing, ref missing);
//                wordApp = null;
//            }
//        }

//        private void Excel2Pdf(string excelDocFile, string pdfDocFile)
//        {
//            object falseObj = false;
//            object trueObj = true;
//            object emptyStr = "";
//            object missing = System.Reflection.Missing.Value;

//            Microsoft.Office.Interop.Excel.Application excelApp = null;
//            try
//            {
//                excelApp = new Microsoft.Office.Interop.Excel.Application();
//                excelApp.Visible = false;
//                Workbook excelWb = null;
//                object qual = XlFixedFormatQuality.xlQualityStandard;

//                try
//                {
//                    excelWb = excelApp.Workbooks.Open(excelDocFile, falseObj, trueObj, missing, missing, emptyStr, trueObj, missing, missing, falseObj, falseObj, missing, falseObj, missing, missing);
//                    excelWb.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, pdfDocFile, qual, trueObj, falseObj, missing, missing, falseObj, missing);
//                }
//                finally
//                {
//                    if (excelWb != null)
//                        excelWb.Close(falseObj, missing, missing);
//                    excelWb = null;
//                }
//            }
//            finally
//            {
//                if (excelApp != null)
//                    excelApp.Quit();
//                excelApp = null;
//            }
//        }
//    }
//}
