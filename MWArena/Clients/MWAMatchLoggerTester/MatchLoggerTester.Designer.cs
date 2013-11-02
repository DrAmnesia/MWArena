namespace MWA.MatchLoggerTester
{
    partial class TestingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestingForm));
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.tbInfo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnMatchSend = new System.Windows.Forms.Button();
            this.tbMatch = new System.Windows.Forms.TextBox();
            this.tbLogs = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbPilotName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 161);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Test Xml Logging";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(119, 156);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbInfo
            // 
            this.tbInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.tbInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbInfo.Location = new System.Drawing.Point(22, 50);
            this.tbInfo.Multiline = true;
            this.tbInfo.Name = "tbInfo";
            this.tbInfo.ReadOnly = true;
            this.tbInfo.Size = new System.Drawing.Size(238, 65);
            this.tbInfo.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 202);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Test Connection";
            // 
            // btnMatchSend
            // 
            this.btnMatchSend.Location = new System.Drawing.Point(119, 195);
            this.btnMatchSend.Name = "btnMatchSend";
            this.btnMatchSend.Size = new System.Drawing.Size(75, 23);
            this.btnMatchSend.TabIndex = 5;
            this.btnMatchSend.Text = "Send";
            this.btnMatchSend.UseVisualStyleBackColor = true;
            this.btnMatchSend.Click += new System.EventHandler(this.btnMatchSend_Click);
            // 
            // tbMatch
            // 
            this.tbMatch.Location = new System.Drawing.Point(25, 226);
            this.tbMatch.Multiline = true;
            this.tbMatch.Name = "tbMatch";
            this.tbMatch.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbMatch.Size = new System.Drawing.Size(235, 309);
            this.tbMatch.TabIndex = 6;
            // 
            // tbLogs
            // 
            this.tbLogs.Location = new System.Drawing.Point(266, 50);
            this.tbLogs.Multiline = true;
            this.tbLogs.Name = "tbLogs";
            this.tbLogs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLogs.Size = new System.Drawing.Size(390, 485);
            this.tbLogs.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(22, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(233, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Test MWO:A MatchLogger Functionality";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(263, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Xml Log";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "PilotName";
            // 
            // tbPilotName
            // 
            this.tbPilotName.Location = new System.Drawing.Point(119, 122);
            this.tbPilotName.Name = "tbPilotName";
            this.tbPilotName.Size = new System.Drawing.Size(141, 20);
            this.tbPilotName.TabIndex = 11;
            this.tbPilotName.Text = "Your Pilot Name";
            this.tbPilotName.Enter += new System.EventHandler(this.tbPilotName_Enter);
            // 
            // CommonLibSample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 561);
            this.Controls.Add(this.tbPilotName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbLogs);
            this.Controls.Add(this.tbMatch);
            this.Controls.Add(this.btnMatchSend);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbInfo);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CommonLibSample";
            this.Text = "MWO:A MatchLogger Tester";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnMatchSend;
        private System.Windows.Forms.TextBox tbMatch;
        private System.Windows.Forms.TextBox tbLogs;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbPilotName;

    }
}

