using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Core;
using Database;


namespace BelegSeitenRepair
{
    public class SeitenRepair
    {
        private static readonly Dictionary<string, string> ArchivPaths = new Dictionary<string, string>();
        private static string _scanPath;

        public static SeitenRepair Instance { get; } = new SeitenRepair();

        SeitenRepair()
        {
            using (
                var dt =
                    Database_Config.Instance.ExecuteQuery(
                        "select wert from Frm_Konfiguration where Schluessel like 'Synios.Public.Folders.ScanFolder%'"))
            {
                if (dt != null && dt.Rows.Count != 0)
                    _scanPath = dt.Rows[0]["wert"].ToString();
            }


            using (var dt2 = Database_Config.Instance.ExecuteQuery("select schluessel,wert from Frm_Konfiguration where Schluessel like 'Synios.Module.DocumentViewer.Archivpfade%'"))
            {
                if (dt2 != null && dt2.Rows.Count != 0)
                {
                    foreach (DataRow row in dt2.Rows)
                        ArchivPaths[row["schluessel"].ToString()] = row["wert"].ToString();
                }
            }
        }

        public ERepairStatus Repair(int belegId, string ablageCode)
        {
            try
            {

                var settings = GetSettings(ablageCode);

                if (string.IsNullOrEmpty(settings.PathToFile))
                {
                    FileLogger.FileLogger.Instance.WriteMessage(
                        $"AblegeCode könnte nicht gefunden werden (BelegID = {belegId})");
                    return ERepairStatus.RepairError;
                }

                var files = GetCheckFiles(settings);

                if (files == null || files.Images.Count == 0)
                    return ERepairStatus.NoRepaired;

                int position = 1;
                foreach (var filesImage in files.Images)
                {
                    var sql =
                        $@"INSERT INTO [dbo].[Dyn_BelegSeiten] ([Beleg_Id] ,[Anzeigen] ,[Position] ,[Ablage] ,[Erfassung] ,[Benutzer]) VALUES ({belegId},1 ,{position} ,'{settings.AblageCode}\{filesImage}' , '{DateTime.Now}' ,'SyniosSupport')";
                    Database_Data.Instance.Execute(sql);
                    ++position;
                }


                return ERepairStatus.Repaired;
            }
            catch (Exception ex)
            {
                FileLogger.FileLogger.Instance.WriteExeption(ex);
                return ERepairStatus.RepairError;
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

            settings.AblageCode = ablageCode;
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

        private CheckFiles GetCheckFiles(AblageInfos settings)
        {
            try
            {
                var checkfiles = new CheckFiles();

                var dir = new DirectoryInfo(settings.PathToFile);
                var files = dir.GetFiles(settings.FileFilter);

                if (files.Length == 0)
                    return null;

                var ext = new List<string> { "pdf", "doc", "docx", "safe" };

                foreach (var file in files)
                {
                    var endString = file.FullName.Substring(file.FullName.LastIndexOf('.') + 1).ToLower();
                    if (ext.Contains(endString))
                        continue;

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