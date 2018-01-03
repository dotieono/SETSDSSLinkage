namespace ClinicSystem
{
    partial class SyncData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SyncData));
            this.lblPublicationName = new System.Windows.Forms.Label();
            this.lblSubscriptionName = new System.Windows.Forms.Label();
            this.pictureBoxStatus = new System.Windows.Forms.PictureBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblLastStatusMessage = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblPublication = new System.Windows.Forms.Label();
            this.lblSubscription = new System.Windows.Forms.Label();
            this.tbLastStatusMessage = new System.Windows.Forms.TextBox();
            this.pbStatus = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPublicationName
            // 
            this.lblPublicationName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPublicationName.AutoSize = true;
            this.lblPublicationName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPublicationName.Location = new System.Drawing.Point(87, 42);
            this.lblPublicationName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPublicationName.Name = "lblPublicationName";
            this.lblPublicationName.Size = new System.Drawing.Size(87, 13);
            this.lblPublicationName.TabIndex = 42;
            this.lblPublicationName.Text = "publication name";
            // 
            // lblSubscriptionName
            // 
            this.lblSubscriptionName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSubscriptionName.AutoSize = true;
            this.lblSubscriptionName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubscriptionName.Location = new System.Drawing.Point(87, 19);
            this.lblSubscriptionName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSubscriptionName.Name = "lblSubscriptionName";
            this.lblSubscriptionName.Size = new System.Drawing.Size(92, 13);
            this.lblSubscriptionName.TabIndex = 41;
            this.lblSubscriptionName.Text = "subscription name";
            // 
            // pictureBoxStatus
            // 
            this.pictureBoxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxStatus.Enabled = false;
            this.pictureBoxStatus.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxStatus.Image")));
            this.pictureBoxStatus.Location = new System.Drawing.Point(38, 78);
            this.pictureBoxStatus.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBoxStatus.MaximumSize = new System.Drawing.Size(45, 32);
            this.pictureBoxStatus.MinimumSize = new System.Drawing.Size(45, 32);
            this.pictureBoxStatus.Name = "pictureBoxStatus";
            this.pictureBoxStatus.Size = new System.Drawing.Size(45, 32);
            this.pictureBoxStatus.TabIndex = 40;
            this.pictureBoxStatus.TabStop = false;
            this.pictureBoxStatus.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(374, 231);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(77, 19);
            this.btnClose.TabIndex = 39;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblLastStatusMessage
            // 
            this.lblLastStatusMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLastStatusMessage.AutoSize = true;
            this.lblLastStatusMessage.Location = new System.Drawing.Point(88, 63);
            this.lblLastStatusMessage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLastStatusMessage.Name = "lblLastStatusMessage";
            this.lblLastStatusMessage.Size = new System.Drawing.Size(40, 13);
            this.lblLastStatusMessage.TabIndex = 38;
            this.lblLastStatusMessage.Text = "Status:";
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(293, 231);
            this.btnStart.Margin = new System.Windows.Forms.Padding(2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(77, 19);
            this.btnStart.TabIndex = 37;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblPublication
            // 
            this.lblPublication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPublication.AutoSize = true;
            this.lblPublication.Location = new System.Drawing.Point(16, 42);
            this.lblPublication.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPublication.Name = "lblPublication";
            this.lblPublication.Size = new System.Drawing.Size(62, 13);
            this.lblPublication.TabIndex = 36;
            this.lblPublication.Text = "Publication:";
            // 
            // lblSubscription
            // 
            this.lblSubscription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSubscription.AutoSize = true;
            this.lblSubscription.Location = new System.Drawing.Point(16, 19);
            this.lblSubscription.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSubscription.Name = "lblSubscription";
            this.lblSubscription.Size = new System.Drawing.Size(68, 13);
            this.lblSubscription.TabIndex = 35;
            this.lblSubscription.Text = "Subscription:";
            // 
            // tbLastStatusMessage
            // 
            this.tbLastStatusMessage.AcceptsReturn = true;
            this.tbLastStatusMessage.AcceptsTab = true;
            this.tbLastStatusMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLastStatusMessage.Location = new System.Drawing.Point(91, 78);
            this.tbLastStatusMessage.Margin = new System.Windows.Forms.Padding(2);
            this.tbLastStatusMessage.Multiline = true;
            this.tbLastStatusMessage.Name = "tbLastStatusMessage";
            this.tbLastStatusMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLastStatusMessage.Size = new System.Drawing.Size(360, 129);
            this.tbLastStatusMessage.TabIndex = 34;
            // 
            // pbStatus
            // 
            this.pbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbStatus.Location = new System.Drawing.Point(92, 211);
            this.pbStatus.Margin = new System.Windows.Forms.Padding(2);
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.Size = new System.Drawing.Size(359, 16);
            this.pbStatus.TabIndex = 33;
            // 
            // SyncData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 318);
            this.Controls.Add(this.lblPublicationName);
            this.Controls.Add(this.lblSubscriptionName);
            this.Controls.Add(this.pictureBoxStatus);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblLastStatusMessage);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblPublication);
            this.Controls.Add(this.lblSubscription);
            this.Controls.Add(this.tbLastStatusMessage);
            this.Controls.Add(this.pbStatus);
            this.Name = "SyncData";
            this.Text = "SyncData";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStatus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPublicationName;
        private System.Windows.Forms.Label lblSubscriptionName;
        private System.Windows.Forms.PictureBox pictureBoxStatus;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblLastStatusMessage;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblPublication;
        private System.Windows.Forms.Label lblSubscription;
        private System.Windows.Forms.TextBox tbLastStatusMessage;
        private System.Windows.Forms.ProgressBar pbStatus;
    }
}