using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace InventoryProduct
{
    public class clsErrorLog
    {
        static string FilePath = null;
        public static void LogError(string Message, string MyClassName, string MyFunctionName, string StackTrace)
        {
            string ErrMsg = string.Empty;

            FilePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ErrorLogPath"]);
            string FolderPath = FilePath + "\\" + System.DateTime.Now.ToString("MM-yyyy");
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            string FileName = FolderPath + "-" + System.DateTime.Now.ToString("dd.MM.yyyy") + "_" + "ErrLog.txt";

            if (!File.Exists(FileName))
            {
                File.Create(FileName).Close();
            }

            // Calculate GMT offset
            int GmtOffset = DateTime.Compare(DateTime.Now, DateTime.UtcNow);
            string GmtPrefix = null;
            if (GmtOffset > 0)
            {
                GmtPrefix = "+";
            }
            else
            {
                GmtPrefix = "";
            }
            // Create DateTime string
            string ErrorDateTime = DateTime.Now.Year.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + " @ " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + " (GMT " + GmtPrefix + GmtOffset.ToString() + ")";
            // Write message to error file
            try
            {
                StreamWriter MsStreamWriter = new StreamWriter(FileName, true);
                MsStreamWriter.WriteLine(Environment.NewLine);
                MsStreamWriter.WriteLine("Date And Time - " + ErrorDateTime);
                MsStreamWriter.WriteLine("Class Name - " + MyClassName);
                MsStreamWriter.WriteLine("Function Name - " + MyFunctionName);
                MsStreamWriter.WriteLine("Error Message - " + Message);
                MsStreamWriter.WriteLine("Stack Trace - " + StackTrace);
                MsStreamWriter.WriteLine("##################################################################");
                MsStreamWriter.Close();
                ErrMsg = Message + " in function '" + MyFunctionName + "' of class file '" + MyClassName + "' " + Environment.NewLine + Environment.NewLine + "";

            }
            catch (Exception ex)
            {
            }
        }
    }
}