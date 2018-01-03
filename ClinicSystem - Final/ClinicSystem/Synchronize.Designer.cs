namespace ClinicSystem
{
    partial class Synchronize
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Synchronize));
            this.pictureBoxConnected = new System.Windows.Forms.PictureBox();
            this.buttonCheckConnectivity = new System.Windows.Forms.Button();
            this.textBoxConnectivity = new System.Windows.Forms.TextBox();
            this.btnPushData = new System.Windows.Forms.Button();
            this.btnPullData = new System.Windows.Forms.Button();
            this.pictureBoxNoConnection = new System.Windows.Forms.PictureBox();
            this.btnCloseSync = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxConnected)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNoConnection)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxConnected
            // 
            this.pictureBoxConnected.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxConnected.Image")));
            this.pictureBoxConnected.Location = new System.Drawing.Point(-10, 26);
            this.pictureBoxConnected.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBoxConnected.Name = "pictureBoxConnected";
            this.pictureBoxConnected.Size = new System.Drawing.Size(166, 166);
            this.pictureBoxConnected.TabIndex = 18;
            this.pictureBoxConnected.TabStop = false;
            this.pictureBoxConnected.Visible = false;
            // 
            // buttonCheckConnectivity
            // 
            this.buttonCheckConnectivity.Location = new System.Drawing.Point(542, 215);
            this.buttonCheckConnectivity.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonCheckConnectivity.Name = "buttonCheckConnectivity";
            this.buttonCheckConnectivity.Size = new System.Drawing.Size(285, 35);
            this.buttonCheckConnectivity.TabIndex = 17;
            this.buttonCheckConnectivity.Text = "Test Connectivity";
            this.buttonCheckConnectivity.UseVisualStyleBackColor = true;
            this.buttonCheckConnectivity.Click += new System.EventHandler(this.buttonCheckConnectivity_Click);
            // 
            // textBoxConnectivity
            // 
            this.textBoxConnectivity.Location = new System.Drawing.Point(225, 26);
            this.textBoxConnectivity.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxConnectivity.Multiline = true;
            this.textBoxConnectivity.Name = "textBoxConnectivity";
            this.textBoxConnectivity.Size = new System.Drawing.Size(600, 158);
            this.textBoxConnectivity.TabIndex = 16;
            // 
            // btnPushData
            // 
            this.btnPushData.Enabled = false;
            this.btnPushData.Location = new System.Drawing.Point(543, 272);
            this.btnPushData.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPushData.Name = "btnPushData";
            this.btnPushData.Size = new System.Drawing.Size(105, 35);
            this.btnPushData.TabIndex = 15;
            this.btnPushData.Text = "Push Data";
            this.btnPushData.UseVisualStyleBackColor = true;
            this.btnPushData.Click += new System.EventHandler(this.btnPushData_Click);
            // 
            // btnPullData
            // 
            this.btnPullData.Location = new System.Drawing.Point(656, 272);
            this.btnPullData.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPullData.Name = "btnPullData";
            this.btnPullData.Size = new System.Drawing.Size(100, 35);
            this.btnPullData.TabIndex = 19;
            this.btnPullData.Text = "Pull Data";
            this.btnPullData.UseVisualStyleBackColor = true;
            // 
            // pictureBoxNoConnection
            // 
            this.pictureBoxNoConnection.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxNoConnection.Image")));
            this.pictureBoxNoConnection.Location = new System.Drawing.Point(18, 46);
            this.pictureBoxNoConnection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBoxNoConnection.Name = "pictureBoxNoConnection";
            this.pictureBoxNoConnection.Size = new System.Drawing.Size(138, 146);
            this.pictureBoxNoConnection.TabIndex = 20;
            this.pictureBoxNoConnection.TabStop = false;
            // 
            // btnCloseSync
            // 
            this.btnCloseSync.Location = new System.Drawing.Point(764, 272);
            this.btnCloseSync.Name = "btnCloseSync";
            this.btnCloseSync.Size = new System.Drawing.Size(75, 36);
            this.btnCloseSync.TabIndex = 21;
            this.btnCloseSync.Text = "Close";
            this.btnCloseSync.UseVisualStyleBackColor = true;
            this.btnCloseSync.Click += new System.EventHandler(this.btnCloseSync_Click);
            // 
            // Synchronize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 408);
            this.Controls.Add(this.btnCloseSync);
            this.Controls.Add(this.pictureBoxNoConnection);
            this.Controls.Add(this.btnPullData);
            this.Controls.Add(this.pictureBoxConnected);
            this.Controls.Add(this.buttonCheckConnectivity);
            this.Controls.Add(this.textBoxConnectivity);
            this.Controls.Add(this.btnPushData);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Synchronize";
            this.Text = "Synchronize";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxConnected)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNoConnection)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxConnected;
        private System.Windows.Forms.Button buttonCheckConnectivity;
        private System.Windows.Forms.TextBox textBoxConnectivity;
        private System.Windows.Forms.Button btnPushData;
        private System.Windows.Forms.Button btnPullData;
        private System.Windows.Forms.PictureBox pictureBoxNoConnection;
        private System.Windows.Forms.Button btnCloseSync;
    }
}