namespace ClinicSystem
{
    partial class EndLinkage
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
            this.btnContinueClient = new System.Windows.Forms.Button();
            this.btnCloseApp = new System.Windows.Forms.Button();
            this.btnCheckin = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnContinueClient
            // 
            this.btnContinueClient.Location = new System.Drawing.Point(175, 77);
            this.btnContinueClient.Name = "btnContinueClient";
            this.btnContinueClient.Size = new System.Drawing.Size(75, 23);
            this.btnContinueClient.TabIndex = 13;
            this.btnContinueClient.Text = "Checkin";
            this.btnContinueClient.UseVisualStyleBackColor = true;
            this.btnContinueClient.Click += new System.EventHandler(this.btnContinueClient_Click);
            // 
            // btnCloseApp
            // 
            this.btnCloseApp.Location = new System.Drawing.Point(256, 77);
            this.btnCloseApp.Name = "btnCloseApp";
            this.btnCloseApp.Size = new System.Drawing.Size(108, 23);
            this.btnCloseApp.TabIndex = 12;
            this.btnCloseApp.Text = "Close Application";
            this.btnCloseApp.UseVisualStyleBackColor = true;
            this.btnCloseApp.Click += new System.EventHandler(this.btnCloseApp_Click);
            // 
            // btnCheckin
            // 
            this.btnCheckin.Location = new System.Drawing.Point(55, 77);
            this.btnCheckin.Name = "btnCheckin";
            this.btnCheckin.Size = new System.Drawing.Size(114, 23);
            this.btnCheckin.TabIndex = 11;
            this.btnCheckin.Text = "New Client";
            this.btnCheckin.UseVisualStyleBackColor = true;
            this.btnCheckin.Click += new System.EventHandler(this.btnCheckin_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(104, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(197, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Linkage process successfully completed";
            // 
            // EndLinkage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 203);
            this.Controls.Add(this.btnContinueClient);
            this.Controls.Add(this.btnCloseApp);
            this.Controls.Add(this.btnCheckin);
            this.Controls.Add(this.label1);
            this.Name = "EndLinkage";
            this.Text = "EndLinkage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCloseApp;
        private System.Windows.Forms.Button btnCheckin;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button btnContinueClient;
    }
}