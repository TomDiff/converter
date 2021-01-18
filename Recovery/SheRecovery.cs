using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Core;
using Database;

namespace Recovery
{
    public class SheRecovery
    {
        private static readonly Dictionary<string, string> ArchivPaths = new Dictionary<string, string>();
        private static string _scanPath;
        public static SheRecovery Instance { get; } = new SheRecovery();

        SheRecovery()
        {
            using (
                var dt =
                    Database_Config.Instance.ExecuteQuery(
                        "select wert from Frm_Konfiguration where Schluessel like 'Synios.Public.Folders.ScanFolder%'"))
            {
                if (dt != null && dt.Rows.Count != 0)
                    _scanPath = dt.Rows[0]["wert"].ToString();
            }


            using (var dt2 =
                Database_Config.Instance.ExecuteQuery("select schluessel,wert from Frm_Konfiguration where Schluessel like 'Synios.Module.DocumentViewer.Archivpfade%'"))
            {
                if (dt2 != null && dt2.Rows.Count != 0)
                {
                    foreach (DataRow row in dt2.Rows)
                        ArchivPaths[row["schluessel"].ToString()] = row["wert"].ToString();
                }
            }
        }

        public ERecoveryStatus CheckAblage(int belegId, string ablageCode)
        {
            try
            {

                var settings = GetSettings(ablageCode);

                if (string.IsNullOrEmpty(settings.PathToFile))
                {
                    FileLogger.FileLogger.Instance.WriteMessage(
                        $"AblegeCode könnte nicht gefunden werden (BelegID = {belegId})");
                    return ERecoveryStatus.RecoveryError;
                }

                var files = GetCheckFiles(settings);

                if (files == null || files.Original.Count == 0 || files.Images.Count == 0)
                    return ERecoveryStatus.NoRecovered;


                Database_Data.Instance.Execute(
                    $"Update Dyn_Beleg SET Seiten = '{files.Images.Count}', OriginalFileName = '{string.Join("|", files.Original.ToArray())}'  WHERE Beleg_ID = '{belegId}'");




                return ERecoveryStatus.Recovered;
            }
            catch (Exception ex)
            {
                FileLogger.FileLogger.Instance.WriteExeption(ex);
                return ERecoveryStatus.RecoveryError;
            }

        }

        private CheckFiles GetCheckFiles(AblageInfos settings)
        {
            try
            {
                var checkfiles = new CheckFiles();

                var dir = new DirectoryInfo(settings.PathToFile);
                var files = dir.GetFiles(settings.FileFilter);

                if (files.Length == 0)
                    return null;

                var withoutSafe = files.Where(f => f.Name.ToLower().Contains(".safe") == false).ToList();


                var ext = new List<string> { "pdf", "doc", "docx" };

                foreach (var file in withoutSafe)
                {
                    var endString = file.FullName.Substring(file.FullName.LastIndexOf('.') + 1).ToLower();
                    if (!ext.Contains(endString))
                        continue;

                    checkfiles.Original.Add(file.Name);


                    var fileBeginnWith = Path.GetFileNameWithoutExtension(file.Name);
                    var expre1 = $@"{fileBeginnWith}[_][0-9]{{4,}}.tif";
                    var expre2 = $@"{fileBeginnWith}[_][0-9]{{4,}}.jpg";

                    var rx1 = new Regex(expre1, RegexOptions.IgnoreCase);
                    var rx2 = new Regex(expre2, RegexOptions.IgnoreCase);
                    foreach (var info in withoutSafe)
                    {

                        if (rx1.IsMatch(info.Name))
                            checkfiles.Images.Add(info.Name);
                        else if (rx2.IsMatch(info.Name))
                            checkfiles.Images.Add(info.Name);
                    }
                }



                return checkfiles;
            }
            catch (Exception ex)
            {
                FileLogger.FileLogger.Instance.WriteExeption(ex);

                return null;
            }
        }

        public ERecoveryStatus RecoveryOriginal(int belegId, string ablageCode)
        {
            try
            {

                var settings = GetSettings(ablageCode);

                if (string.IsNullOrEmpty(settings.PathToFile))
                {
                    FileLogger.FileLogger.Instance.WriteMessage(
                        $"AblegeCode könnte nicht gefunden werden (BelegID = {belegId})");
                    return ERecoveryStatus.RecoveryError;
                }

                var list = GetFiles(settings);

                if (list.Count == 0)
                    return ERecoveryStatus.NoRecovered;


                Database_Data.Instance.Execute(
                    $"Update Dyn_Beleg SET Seiten = '{list.Count}', OriginalFileName = null  WHERE Beleg_ID = '{belegId}'");


                return ERecoveryStatus.Recovered;
            }
            catch (Exception ex)
            {
                FileLogger.FileLogger.Instance.WriteExeption(ex);
                return ERecoveryStatus.RecoveryError;
            }
        }

        private AblageInfos GetSettings(string belegAblagecode)
        {
            var settings = new AblageInfos();


            var ablage = belegAblagecode;
            ablage = ablage.Replace(":", "");
            var code = ablage.Split('\\');
            if (code.Length != 6)
                return settings;

            var ablageCode = $"{code[0]}\\{code[1]}\\{code[2]}\\{code[3]}\\{code[4]}";

            var sourcePath = GetArchivPath(ablageCode);

            if (Directory.Exists(sourcePath) == false)
                return settings;

            settings.AblageCode = $"{code[0]}\\{code[1]}\\ORIGINAL\\{code[3]}\\{code[4]}";
            settings.PathToFile = sourcePath;
            settings.FileFilter = code[5];

            return settings;

        }

        private static string GetArchivPath(string ablagePfad)
        {
            var splitAblagepfad = ablagePfad.Split(':');
            if (splitAblagepfad[0] == "SCAN")
            {
                var confPfad = Path.Combine(_scanPath, splitAblagepfad[1]);
                return confPfad;
            }
            else
            {
                ablagePfad = ablagePfad.Replace(":", "");
            }

            foreach (var s in ArchivPaths.Values)
            {
                var completePfad = Path.Combine(s, ablagePfad);
                if (Directory.Exists(completePfad))
                    return Path.Combine(s, ablagePfad);
            }
            return ablagePfad;
        }

        private List<string> GetFiles(AblageInfos settigns)
        {
            var list = new List<string>();
            var dir = new DirectoryInfo(settigns.PathToFile);
            var files = dir.GetFiles(settigns.FileFilter);

            if (files.Length == 0)
                return list;

            var ext = new List<string> { "pdf", "doc", "docx" };
            var fileBeginnWith = settigns.FileFilter.Replace("*.*", "");

            var expre = $@"{fileBeginnWith}[0-9]{{3}}[_][0-9]{{4,}}.tif";

            DeleteOldFiles(files, expre);

            var expre2 = $@"{fileBeginnWith}[0-9]{{3}}[_][0-9]{{4,}}.jpg";

            DeleteOldFiles(files, expre2);

            foreach (var file in files)
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
            var rx = new Regex(expre, RegexOptions.IgnoreCase);
            foreach (var file in files)
            {
                //Test für Löschen von alte Datein
                if (rx.IsMatch(file.Name))
                    File.Delete(file.FullName);
            }
        }
        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="belegId"></param>
        /// <param name="ablageCode"></param>
        /// <param name="seiten"></param>
        /// <returns></returns>
        public ERecoveryStatus CheckAblageWihtoutOriginal(int belegId, string ablageCode, int seiten)
        {
            try
            {

                var settings = GetSettings(ablageCode);

                if (string.IsNullOrEmpty(settings.PathToFile))
                {
                    FileLogger.FileLogger.Instance.WriteMessage(
                        $"AblegeCode könnte nicht gefunden werden (BelegID = {belegId})");
                    return ERecoveryStatus.RecoveryError;
                }

                var files = GetCheckFilesWihtoutOriginal(settings);

                if (files == null || files.Images.Count == 0)
                    return ERecoveryStatus.NoRecovered;


                if (seiten == files.Images.Count)
                    return ERecoveryStatus.NoRecovered;

                Database_Data.Instance.Execute(
                    $"Update Dyn_Beleg SET Seiten = '{files.Images.Count}'  WHERE Beleg_ID = '{belegId}'");




                return ERecoveryStatus.Recovered;
            }
            catch (Exception ex)
            {
                FileLogger.FileLogger.Instance.WriteExeption(ex);
                return ERecoveryStatus.RecoveryError;
            }

        }

        private CheckFiles GetCheckFilesWihtoutOriginal(AblageInfos settings)
        {
            try
            {
                var checkfiles = new CheckFiles();

                var dir = new DirectoryInfo(settings.PathToFile);
                var files = dir.GetFiles(settings.FileFilter);

                if (files.Length == 0)
                    return null;

                var ext = new List<string> { "tif", "tiff", "jpg", "jpeg" };

                var filesWithoutUnderline = files.Where(f => f.Name.Contains("_") == false).ToList();

                foreach (var file in filesWithoutUnderline)
                {
                    var endString = file.FullName.Substring(file.FullName.LastIndexOf('.') + 1).ToLower();
                    if (!ext.Contains(endString))
                        continue;

                    checkfiles.Original.Add(file.Name);

                    checkfiles.Images.Add(file.Name);
                }



                return checkfiles;
            }
            catch (Exception ex)
            {
                FileLogger.FileLogger.Instance.WriteExeption(ex);

                return null;
            }
        }


    }
}