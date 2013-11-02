using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MWA.MatchLoggerTester
{
    static class Program
    {
 

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var clform = new TestingForm();
            string build = (System.Configuration.ConfigurationManager.AppSettings["build"] == null |
                            System.Configuration.ConfigurationManager.AppSettings["build"] == "")
                               ? "0.1"
                               : System.Configuration.ConfigurationManager.AppSettings["build"];
            clform.Text = String.Format("MWO:A MatchLogger Tester{0}", build);
            Application.Run(clform);
        }
    }
}
