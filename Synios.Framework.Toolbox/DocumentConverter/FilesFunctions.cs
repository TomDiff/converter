using System;
using System.Collections.Generic;
using System.IO;

namespace Synios.Framework.Toolbox.DocumentConverter
{
    internal class FilesFunctions
    {
        //Löscht der Inhalt des Ordners.
        public static void DeleteFilesInFolder(string PathFolder, string extension)
        {
            if (Directory.Exists(PathFolder) == false)
                return;

            string[] filePaths = Directory.GetFiles(PathFolder, string.Format("Tmp_*{0}", extension));
            Array.ForEach(filePaths, filePath =>
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    FileLogger.FileLogger.Instance.WriteExeption(ex);
                }
            });
        }

        //Erzeugt einen Ordner unter den Temp-Ordner des aktuellen Benutzers
        public static string CreateFolder(FileInfo infoFile)
        {
            try
            {
                string dir = string.Format(@"{0}{1}\{2}", Path.GetTempPath(), "SYNIOS", Path.GetFileNameWithoutExtension(infoFile.FullName));
                if (Directory.Exists(dir) == false)
                    Directory.CreateDirectory(dir);


                return dir;
            }
            catch (Exception ex)
            {
                FileLogger.FileLogger.Instance.WriteExeption(ex);

                return string.Empty;
            }
        }

        //Gib eindeutigen Name für die Datei zurück.
        public static string GetRandomName(string path, FileInfo infoFile, bool TempPrefix, string extension)
        {
            string fileName = "";
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(infoFile.FullName);
            int index = 1;
            do
            {
                if (TempPrefix)
                    fileName = string.Format(@"{0}\{1}{2}_{3:00000}{4}", path, "Tmp_", fileNameWithoutExtension, index, extension);
                else
                    fileName = string.Format(@"{0}\{1}_{2:00000}{3}", path, fileNameWithoutExtension, index, extension);

                ++index;
            } while (File.Exists(fileName) == true);

            return fileName;
        }

     }//class End
}
