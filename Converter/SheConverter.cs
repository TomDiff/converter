using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using Database;
using Synios.Framework.Toolbox.DocumentConverter;
using Synios.Framework.Toolbox.DocumentConverter.Enums;
using Synios.Framework.Toolbox.DocumentConverter.Interfaces;

namespace Converter
{

    public class SheConverter
    {
        private static string _pathToOriginalTargetFolder;
        private static string _scanPath;
        private static readonly Dictionary<string, string> ArchivPaths = new Dictionary<string, string>();
        private static readonly SheConverter instance = new SheConverter();
        private static IDocumentConverter _converter = new ConvertDocument();
        private static readonly DocumentParam ConvertParam = new DocumentParam();
        private static eToConvertTyp _documentConverterDefault;

        public static SheConverter Instance
        {
            get
            {
                return instance;
            }
        }

        SheConverter()
        {
            using (
                var dt =
                    Database_Config.Instance.ExecuteQuery(
                        "select wert from Frm_Konfiguration where Schluessel like 'Synios.Public.Folders.ScanFolder%'"))
            {
                if (dt != null && dt.Rows.Count != 0)
                    _scanPath = dt.Rows[0]["wert"].ToString();
            }


            using (DataTable dt2 = Database_Config.Instance.ExecuteQuery("select schluessel,wert from Frm_Konfiguration where Schluessel like 'Synios.Module.DocumentViewer.Archivpfade%'"))
            {
                if (dt2 != null && dt2.Rows.Count != 0)
                {
                    foreach (DataRow row in dt2.Rows)
                        ArchivPaths[row["schluessel"].ToString()] = row["wert"].ToString();
                }
            }

            _documentConverterDefault = eToConvertTyp.Jpg;
            using (DataTable dt3 = Database_Config.Instance.ExecuteQuery("select schluessel,wert from Frm_Konfiguration where Schluessel like 'Synios.Client.Common.Module.MediaImport.DocumentConverterDefault'"))
            {
                if (dt3 != null && dt3.Rows.Count != 0)
                    _documentConverterDefault = ConvertToEnum<eToConvertTyp>(dt3.Rows[0]["wert"]);
            }


            _pathToOriginalTargetFolder = string.Empty;
        }

        private T ConvertToEnum<T>(object o)
        {
            T enumVal = (T)Enum.Parse(typeof(T), o.ToString());
            return enumVal;
        }


        public EConverterStatus Reconvert(int belegId, string ablageCode)
        {
            try
            {

                ConverterInfos settings = GetSettings(ablageCode);

                if (string.IsNullOrEmpty(settings.PathToFile))
                {
                    FileLogger.FileLogger.Instance.WriteMessage(
                        $"AblegeCode könnte nicht gefunden werden (BelegID = {belegId})");
                    return EConverterStatus.ConverterError;
                }

                List<string> list = GetFiles(settings);

                if (list.Count == 0)
                    return EConverterStatus.NoConverted;

                foreach (var path in list)
                {

                    ConvertParam.FilePath = path;
                    ConvertParam.ConTyp = _documentConverterDefault;
                    if (path.EndsWith(".pdf", StringComparison.CurrentCultureIgnoreCase))
                        ConvertParam.Doctyp = eDocumentTyp.PDF;
                    else if (path.EndsWith(".doc", StringComparison.CurrentCultureIgnoreCase) || path.EndsWith(".docx", StringComparison.CurrentCultureIgnoreCase) || path.EndsWith(".rtf", StringComparison.CurrentCultureIgnoreCase))
                        ConvertParam.Doctyp = eDocumentTyp.WORD;
                    else
                        ConvertParam.Doctyp = eDocumentTyp.NONE;


                    int count = ConvertFile(ConvertParam);
                    if (count == 0)
                    {
                        FileLogger.FileLogger.Instance.WriteMessage(
                            $"Folgende Dateie könnte nicht konvertiert werden: {path}");
                        continue;
                    }

                    if (_pathToOriginalTargetFolder != string.Empty)
                    {
                        if (CopyOriginalFileToTarget(settings, path) == false)
                        {
                            FileLogger.FileLogger.Instance.WriteMessage(
                                $"Folgende Dateie könnte nicht kopiert werden: {path}");
                            continue;
                        }
                    }

                }

                return EConverterStatus.Converted;
            }
            catch (Exception ex)
            {
                FileLogger.FileLogger.Instance.WriteExeption(ex);
                return EConverterStatus.ConverterError;
            }
        }

        public EConverterStatus ConvertFilesIfNeeded(int belegId, string ablageCode)
        {
            try
            {

                ConverterInfos settings = GetSettings(ablageCode);

                if (string.IsNullOrEmpty(settings.PathToFile))
                {
                    FileLogger.FileLogger.Instance.WriteMessage(
                        $"AblegeCode könnte nicht gefunden werden (BelegID = {belegId})");
                    return EConverterStatus.ConverterError;
                }

                List<string> list = GetFiles(settings);

                if (list.Count == 0)
                    return EConverterStatus.NoConverted;

                int totalCount = 0;
                long fileSize = 0;
                List<string> files = new List<string>();
                foreach (var path in list)
                {
                    ConvertParam.FilePath = path;
                    ConvertParam.ConTyp = _documentConverterDefault;
                    if (path.EndsWith(".pdf", StringComparison.CurrentCultureIgnoreCase))
                        ConvertParam.Doctyp = eDocumentTyp.PDF;
                    else if (path.EndsWith(".doc", StringComparison.CurrentCultureIgnoreCase) || path.EndsWith(".docx", StringComparison.CurrentCultureIgnoreCase) || path.EndsWith(".rtf", StringComparison.CurrentCultureIgnoreCase))
                        ConvertParam.Doctyp = eDocumentTyp.WORD;
                    else
                        ConvertParam.Doctyp = eDocumentTyp.NONE;


                    int count = ConvertFile(ConvertParam);
                    if (count == 0)
                    {
                        FileLogger.FileLogger.Instance.WriteMessage(
                            $"Folgende Dateie könnte nicht konvertiert werden: {path}");
                        continue;
                    }

                    if (_pathToOriginalTargetFolder != string.Empty)
                    {
                        if (CopyOriginalFileToTarget(settings, path) == false)
                        {
                            FileLogger.FileLogger.Instance.WriteMessage(
                                $"Folgende Dateie könnte nicht kopiert werden: {path}");
                            continue;
                        }
                    }

                    totalCount += count;
                    FileInfo info = new FileInfo(path);
                    files.Add(info.Name);
                    fileSize += info.Length;
                }

                if (totalCount == 0 || files.Count == 0)
                    return EConverterStatus.NoConverted;

                totalCount = totalCount + GetCountOhterFiles(settings, ConvertParam);
                Database_Data.Instance.Execute(
                    $"Update Dyn_Beleg SET Seiten = '{totalCount}', OriginalFileName = '{string.Join("|", files.ToArray())}'  WHERE Beleg_ID = '{belegId}'");

                if (fileSize > 0)
                    UpdateIsoImage(fileSize);

                return EConverterStatus.Converted;
            }
            catch (Exception ex)
            {
                FileLogger.FileLogger.Instance.WriteExeption(ex);
                return EConverterStatus.ConverterError;
            }
        }
        private void UpdateIsoImage(long fileSize)
        {
            try
            {
                const string select = "SELECT top 1 * FROM Sys_IsoImage order by CdNr desc";
                DataTable table = Database_Config.Instance.ExecuteQuery(select);

                if (table.Rows.Count == 0)
                {
                    FileLogger.FileLogger.Instance.WriteMessage("Kein eintrag in Sys_IsoImage gefunden gefunden");
                    return;
                }

                long currentSize = table.Rows[0].Field<string>("UsedSize") == null ? 0 : Convert.ToInt64(table.Rows[0].Field<string>("UsedSize"));
                currentSize += fileSize;

                string update =
                    $"UPDATE Sys_IsoImage SET UsedSize = '{currentSize}' WHERE CdNr='{table.Rows[0]["CdNr"]}'";

                Database_Config.Instance.Execute(update);


                table.Dispose();
            }
            catch (Exception ex)
            {
                FileLogger.FileLogger.Instance.WriteExeption(ex);
            }

        }

        private int GetCountOhterFiles(ConverterInfos settings, DocumentParam param)
        {
            List<FileInfo> fileList = new List<FileInfo>();
            DirectoryInfo dir = new DirectoryInfo(settings.PathToFile);
            FileInfo[] files = dir.GetFiles(settings.FileFilter);

            if (files.Length == 0)
                return fileList.Count;

            string fileBeginnWith = settings.FileFilter.Replace("*.*", "");
            string expre = $@"{fileBeginnWith}[0-9]{{3}}[_][0-9]{{4,}}.{param.ConTyp.ToString().ToLower()}";

            Regex rx = new Regex(expre, RegexOptions.IgnoreCase);
            Array.ForEach(files, file =>
            {
                if (rx.IsMatch(file.Name) == false)
                    fileList.Add(file);
            });

            List<string> ext = new List<string> { "pdf", "doc", "docx", "rtf", "safe" };

            int count = 0;
            foreach (FileInfo file in fileList)
            {
                var endString = file.FullName.Substring(file.FullName.LastIndexOf('.') + 1).ToLower();
                if (ext.Contains(endString))
                    continue;

                ++count;
            }

            return count;
        }

        private bool CopyOriginalFileToTarget(ConverterInfos settings, string file)
        {
            try
            {
                string target = Path.Combine(_pathToOriginalTargetFolder, settings.AblageCode);
                DirectoryInfo dir = new DirectoryInfo(target);
                if (dir.Exists == false)
                    dir.Create();

                FileInfo info = new FileInfo(file);
                target = Path.Combine(target, info.Name);
                File.Copy(file, target, true);
            }
            catch (Exception ex)
            {
                FileLogger.FileLogger.Instance.WriteMessage($"Folgende Dateie könnte nicht kopiert werden: {file}");
                FileLogger.FileLogger.Instance.WriteExeption(ex);
                return false;
            }
            return true;
        }

        private static int ConvertFile(DocumentParam param)
        {
            try
            {
                List<string> list = param.Doctyp != eDocumentTyp.NONE
                    ? _converter.ConvertToImage(param)
                    : new List<string>();

                return list.Count;
            }
            catch (Exception )
            {
                return 0;
            }
        }

        private ConverterInfos GetSettings(string belegAblagecode)
        {
            ConverterInfos settings = new ConverterInfos();
            //DataTable dt = new DataTable();
            //dt = SEPAVASDatabase_Data.Instance.ExecuteQuery(string.Format("Select * from Dyn_Beleg Where Beleg_ID = {0}", id));
            //if (dt.Rows.Count == 0)
            //    return settings;

            //string ablage = dt.Rows[0]["Ablagecode"].ToString();

            string ablage = belegAblagecode;
            ablage = ablage.Replace(":", "");
            string[] code = ablage.Split('\\');
            if (code.Length != 6)
                return settings;

            string ablageCode = $"{code[0]}\\{code[1]}\\{code[2]}\\{code[3]}\\{code[4]}";
            string sourcePath = GetArchivPath(ablageCode);

            if (Directory.Exists(sourcePath) == false)
                return settings;

            settings.AblageCode = $"{code[0]}\\{code[1]}\\ORIGINAL\\{code[3]}\\{code[4]}";
            settings.PathToFile = sourcePath;
            settings.FileFilter = code[5];

            return settings;

        }

        private static string GetArchivPath(string ablagePfad)
        {
            string[] splitAblagepfad = ablagePfad.Split(':');
            if (splitAblagepfad[0] == "SCAN")
            {
                string confPfad = Path.Combine(_scanPath, splitAblagepfad[1]);
                return confPfad;
            }
            else
            {
                ablagePfad = ablagePfad.Replace(":", "");
            }

            foreach (string s in ArchivPaths.Values)
            {
                string completePfad = Path.Combine(s, ablagePfad);
                if (Directory.Exists(completePfad))
                    return Path.Combine(s, ablagePfad);
            }
            return ablagePfad;
        }

        private List<string> GetFiles(ConverterInfos settigns)
        {
            List<string> list = new List<string>();
            DirectoryInfo dir = new DirectoryInfo(settigns.PathToFile);
            FileInfo[] files = dir.GetFiles(settigns.FileFilter);

            if (files.Length == 0)
                return list;

            List<string> ext = new List<string> { "pdf", "doc", "docx" };
            string fileBeginnWith = settigns.FileFilter.Replace("*.*", "");
            string expre = $@"{fileBeginnWith}[0-9]{{3}}[_][0-9]{{4,}}.tif";

            // DeleteOldFiles(files, expre);

            string expre2 = $@"{fileBeginnWith}[0-9]{{3}}[_][0-9]{{4,}}.jpg";

            // DeleteOldFiles(files, expre2);

            foreach (FileInfo file in files)
            {
                var endString = file.FullName.Substring(file.FullName.LastIndexOf('.') + 1).ToLower();
                if (!ext.Contains(endString))
                    continue;
                list.Add(file.FullName);
            }

            return list;
        }

        private void DeleteOldFiles(FileInfo[] files, string expre)
        {
            Regex rx = new Regex(expre, RegexOptions.IgnoreCase);
            foreach (FileInfo file in files)
            {

                //Test für Löschen von alte Datein
                if (rx.IsMatch(file.Name))
                    File.Delete(file.FullName);
            }
        }

    }
}