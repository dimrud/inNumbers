using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InNumbers
{
    static class Program
    {
        public static string currentUserId = string.Empty;
        public static string currentUserFullName = string.Empty;
        public static string fileName = string.Empty;
        public static string filePath = string.Empty;
        public static string fileNameClientTrack = string.Empty;
        public static string filePathClientTrack = string.Empty;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Check for sigle running app
            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Length > 1)
            {
                MessageBox.Show("Can't run same applicaiotn twise !!!");
                Application.Exit();
            }

            fileName = ConfigurationManager.AppSettings["fileName"];
            filePath = ConfigurationManager.AppSettings["filePath"];
            fileNameClientTrack = ConfigurationManager.AppSettings["fileNameClientTrack"];
            filePathClientTrack = ConfigurationManager.AppSettings["filePathClientTrack"];

            //Check if DB file exists
            try
            {
                if (File.Exists(filePath + "\\" + fileName))
                {
                    Application.Run(new Login());
                }
                else
                {
                    MessageBox.Show("Please check for DataBase file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
    }
}
