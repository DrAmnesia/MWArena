using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MWA.Models;
using MatchLogger;
using Common;
 
namespace MWA.MatchLoggerTester
{
    public partial class TestingForm : Form
    {
        public MatchStat match;
      
        public SampleTrackable MwoATrackable;
        public TestingForm()
        {
            InitializeComponent();
            
            string testername = (tbPilotName.Text == null || tbPilotName.Text == "") ? "TestPilot" : tbPilotName.Text;
 
            tbInfo.AppendText("Log file is set in MwoALogWarriorTester.exe.config." + System.Environment.NewLine + "Check the Windows.Application Event Log and/or the log file for debug information.");
            Guid t = System.Guid.NewGuid();
            Status s = new Status();
            MwoATrackable = new SampleTrackable(t, s);
            MwoATrackable.Status.Update(Codes.CONTINUE, "SampleTrackable Initialized... Cehcking .Net Framework Version...");

            string fwver = Common.Util.DotNetFrameworkHelper.GetVersionFromRegistry();
            MwoATrackable.Status.Update(Codes.CONTINUE, fwver);
            tbMatch.AppendText(fwver);
            fwver = Common.Util.DotNetFrameworkHelper.Get45or451FromRegistry();
            MwoATrackable.Status.Update(Codes.CONTINUE, fwver);
            tbMatch.AppendText(fwver);
            string msg = Environment.NewLine;
            if (!tbMatch.Text.Contains("Full  4.5"))
            {
                 msg += "MISSING .NET FRAMEWORK 4.5"+ Environment.NewLine +
                                    "Install .net 4.5 framework from : http://www.microsoft.com/en-us/download/details.aspx?id=30653";
                MwoATrackable.Status.Update(Codes.ERROR, msg);
    btnMatchSend.Enabled = false;
            }
            else
            {
                msg += ".NET FRAMEWORK VERSION 4.5 CHECK PASSED";
 MwoATrackable.Status.Update(Codes.SUCCESS, msg);
            }

            msg = Environment.NewLine + MwoATrackable.Status.CurrentStatusEntry.Message;
            tbMatch.AppendText(Environment.NewLine + msg);
            tbMatch.AppendText(Environment.NewLine + MwoATrackable.Status.StatusText);
            tbLogs.Text = MwoATrackable.Status.ToString();
            MwoATrackable.PublishStatus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MwoATrackable.Status.Update(Codes.SUCCESS, "MWO:A LOGWARRIOR LOGGER TEST PASSED...");
            string s = Environment.NewLine + MwoATrackable.Status.CurrentStatusEntry.Message;
            tbMatch.AppendText(Environment.NewLine + s);
            tbMatch.AppendText(Environment.NewLine + MwoATrackable.Status.StatusText);
            tbLogs.Text = MwoATrackable.Status.ToString();
            string testername = (tbPilotName.Text == null || tbPilotName.Text == "") ? "TestPilot" : tbPilotName.Text;
            MatchLogger.MatchLogger.SetPlayerName(testername);
            tbApiUrl.Text = MatchLogger.MatchLogger.GetApiUrl();
            MwoATrackable.PublishStatus();
        }

     

        private void btnMatchSend_Click(object sender, EventArgs e)
        {

        }

        private void tbPilotName_Enter(object sender, EventArgs e) { if (tbPilotName.Text == "Your Pilot Name") tbPilotName.Text = ""; }

        private void button2_Click(object sender, EventArgs e)
        {
          /*  
            try
            {
                string testername = (tbPilotName.Text == null || tbPilotName.Text == "") ? "TestPilot" : tbPilotName.Text;
                MatchLogger.MatchLogger.SetPlayerName(testername);
                tbApiUrl.Text = MatchLogger.MatchLogger.GetApiUrl("LoggedMatch");
                MwoATrackable.Status.Update(Codes.CONTINUE,
                                            String.Format("Creating test match for pilot: {0}", testername));
 
                MatchLogger.MatchLogger.LoggedMatch lm = new MatchLogger.MatchLogger.LoggedMatch();
                lm.AssociationName = "TEST";
                lm.PublishingUserName = "DrAmnesia";
                lm.LoadTestMatchStats();
                MatchLogger.MatchLogger.httpPostLoggedMatch(lm);
                string matchinfo = String.Format("Sending Full LoggedMatch... Pilot:{0}, Association:{1}",testername,
                                                 lm.AssociationName);
                MwoATrackable.Status.Update(Codes.CONTINUE, matchinfo);
   
  
                MwoATrackable.Status.Update(Codes.SUCCESS, "LoggedMatch sent, contact DrAmnesia for confirmation.");
            }
            catch (Exception ex)
            {
                MwoATrackable.Status.FromException(ex);
            }
            finally
            {
                string s = Environment.NewLine + MwoATrackable.Status.CurrentStatusEntry.Message;
                tbMatch.AppendText(Environment.NewLine + s);
                tbMatch.AppendText(Environment.NewLine + MwoATrackable.Status.StatusText);
                tbLogs.Text = MwoATrackable.Status.ToString();
                MwoATrackable.PublishStatus();
            }*/
        }

  
    }
     
}
