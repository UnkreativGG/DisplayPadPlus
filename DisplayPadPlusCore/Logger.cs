// Decompiled with JetBrains decompiler
// Type: DisplayPad.SDK.Helper.Logger
// Assembly: DisplayPad.SDK, Version=1.0.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 7DCCFAF8-4DB1-4F98-9EC2-478463B0B913
// Assembly location: C:\Users\Unkreativ\.nuget\packages\displaypad.sdk\1.0.6\lib\net6.0-windows7.0\DisplayPad.SDK.dll
// XML documentation location: C:\Users\Unkreativ\.nuget\packages\displaypad.sdk\1.0.6\lib\net6.0-windows7.0\DisplayPad.SDK.xml

#nullable disable
namespace DisplayPadPlusCore
{
    internal static class Logger
    {
        private static string _logPath;

        public static bool IsBusy { get; private set; }

        /// <summary>
        /// Folder path to save log files.
        /// (If not passed then will save files inside application folder)
        /// </summary>
        public static string LogPath
        {
            get => Logger._logPath;
            set => Logger._logPath = value;
        }

        private static void Log(string Type, Exception ex, string title = null)
        {
            if (Logger.IsBusy)
                Thread.Sleep(1000);
            try
            {
                Logger.IsBusy = true;
                DateTime now = DateTime.Now;
                string str1 = now.ToShortDateString().ToString();
                now = DateTime.Now;
                string str2 = now.ToLongTimeString().ToString();
                string str3 = "TIME :" + str1 + " " + str2;
                now = DateTime.Now;
                string str4 = now.Year.ToString();
                now = DateTime.Now;
                string str5 = now.Month.ToString();
                now = DateTime.Now;
                string str6 = now.Day.ToString();
                string str7 = str4 + str5 + str6;
                string str8 = string.IsNullOrEmpty(Logger.LogPath) ? AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\" : Logger.LogPath;
                Task.Run((Action)(() => Logger.DeleteOldLogs(AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\")));
                if (!Directory.Exists(str8))
                    Directory.CreateDirectory(str8);
                using (StreamWriter streamWriter = new StreamWriter(Path.Combine(str8, str7 + "__DisplayPadSDK.log"), true))
                {
                    streamWriter.WriteLine();
                    streamWriter.WriteLine("***************************************************************************************************************");
                    if (!string.IsNullOrEmpty(title))
                        streamWriter.WriteLine("TITLE == " + title);
                    streamWriter.WriteLine("TYPE == " + Type);
                    streamWriter.WriteLine(str3);
                    streamWriter.WriteLine("==> MESSAGE : " + (string.IsNullOrEmpty(ex.Message) ? string.Empty : ex.Message.Trim()));
                    streamWriter.WriteLine("==> STACK TRACE : " + (string.IsNullOrEmpty(ex.StackTrace) ? string.Empty : ex.StackTrace.Trim()));
                    if (ex.InnerException != null)
                    {
                        streamWriter.WriteLine("==> INNER EXCEPTION : " + ex.InnerException?.ToString());
                        streamWriter.WriteLine("==> INNER STACK TRACE : " + (string.IsNullOrEmpty(ex.InnerException.StackTrace.ToString()) ? string.Empty : ex.InnerException.StackTrace.ToString().Trim()));
                    }
                    streamWriter.WriteLine("***************************************************************************************************************");
                    streamWriter.Flush();
                }
            }
            catch (Exception ex1)
            {
            }
            finally
            {
                Logger.IsBusy = false;
            }
        }

        /// <summary>Write exception details in log file</summary>
        /// <param name="ex"> exception to log</param>
        public static void LogError(Exception ex, string title = null)
        {
            Logger.Log("ERROR", ex, title);
        }

        /// <summary>Write any text in log file</summary>
        /// <param name="message"> message text to log</param>
        public static void LogMessage(string message, string title = null)
        {
            Logger.Log("MESSAGE", new Exception(message), title);
        }

        /// <summary>Deletes the old files</summary>
        /// <param name="dirName"></param>
        private static void DeleteOldLogs(string dirName)
        {
            foreach (string file in Directory.GetFiles(dirName))
            {
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.LastAccessTime < DateTime.Now.AddDays(-3.0))
                    fileInfo.Delete();
            }
        }
    }
}
