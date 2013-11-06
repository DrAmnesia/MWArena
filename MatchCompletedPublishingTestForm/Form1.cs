using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MatchLogger;

namespace MatchCompletedPublishingTestForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoggedMatch lm = new LoggedMatch();
            lm.AssociationName = "TEST";
            lm.PublishingUserName = "DrAmnesia";
            lm.EnemyMatchStats = new List<MatchStat>();
            lm.FriendlyMatchStats = new List<MatchStat>();
            MatchLogger.MatchCompletedPublisher.Publish(lm);

        }
    }
}
