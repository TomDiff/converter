using System;
using System.IO;
using System.Reflection;


namespace FileLogger
{
    public sealed class FileLogger
    {
        static string Location = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        static readonly object locker = new object();
        static readonly string FilePath = Path.Combine(Location, @"Log\SHEConverter.log");

        private static volatile FileLogger instance;
        private static object syncRoot = new Object();
        private static string LogInfo = string.Empty;

        private FileLogger()
        {
            string path = Path.Combine(Location, @"Log");
            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);

            LogInfo = string.Empty;

        }

        public static FileLogger Instance
        {
            get
            {
                if (instance != null)
                    return instance;

                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new FileLogger();
                }


                return instance;
            }
        }

        public void WriteExeption(Exception exc)
        {
            lock (locker)
            {
                StreamWriter writer = null;
                try
                {

                    writer = new StreamWriter(FilePath, true);

                    string message = String.Format("[{0}] : {1} ",
                                                    DateTime.Now.ToString(),
                                                    exc.ToString());

                    writer.WriteLine(message);
                    writer.Close();
                    writer = null;


                }
                catch
                {
                    // throw;
                }
                finally
                {
                    if (writer != null)
                        writer.Dispose();
                }
            }
        }

        public void WriteMessage(string text)
        {
            lock (locker)
            {
                StreamWriter writer = null;
                try
                {
                    writer = new StreamWriter(FilePath, true);
                    string message = String.Format("[{0}] {1}",
                                                  DateTime.Now.ToString(),
                                                  text);

                    writer.WriteLine(message);
                    writer.Close();
                    writer = null;
                }
                catch
                {
                    //throw;
                }
                finally
                {
                    if (writer != null)
                        writer.Dispose();
                }
            }
        }

        public void DeleteLog()
        {
            try
            {
                lock (locker)
                {
                    System.IO.File.WriteAllText(FilePath, string.Empty);
                }
            }
            catch (Exception)
            {
                //throw;
            }
        }

        public string ReadLog()
        {
            LogInfo = string.Empty;
            StreamReader reader = null;
            lock (locker)
            {
                try
                {
                    reader = new StreamReader(FilePath);
                    LogInfo = reader.ReadToEnd();
                    reader.Close();
                    return LogInfo;
                }
                catch (Exception)
                {
                    return LogInfo;
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                        reader.Dispose();
                    }
                }
            }
        }
    }
}
