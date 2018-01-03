using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using Microsoft.SqlServer.Replication;
using Microsoft.SqlServer.Management.Common;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ClinicSystem
{
    public partial class SyncData : Form
    {

        string publicationName = Convert.ToString(ConfigurationManager.AppSettings["PublicationName"]);
        string publisherName = Convert.ToString(ConfigurationManager.AppSettings["PublisherName"]);
        string subscriberName = Convert.ToString(ConfigurationManager.AppSettings["SubscriberName"]);
        string publicationDbName = Convert.ToString(ConfigurationManager.AppSettings["PublicationDbName"]);
        string subscriptionDbName = Convert.ToString(ConfigurationManager.AppSettings["SubscriptionDbName"]);
        string IpAddress = Convert.ToString(ConfigurationManager.AppSettings["PublisherIP"]);
        string srvLogin = Convert.ToString(ConfigurationManager.AppSettings["PublisherLogin"]);
        string srvPass = Convert.ToString(ConfigurationManager.AppSettings["PublisherPass"]);

        // Merge agent
        MergeSynchronizationAgent agent;

        // Sync BackgroundWorker
        BackgroundWorker syncBackgroundWorker;

        public SyncData()
        {
            InitializeComponent();

            lblSubscriptionName.Text = "[" + subscriptionDbName + "] - [" + publisherName + "] - [" + publicationDbName + "]";
            lblPublicationName.Text = publicationName;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            syncBackgroundWorker = new BackgroundWorker();
            syncBackgroundWorker.WorkerReportsProgress = true;
            syncBackgroundWorker.DoWork += new DoWorkEventHandler(syncBackgroundWorker_DoWork);
            syncBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(syncBackgroundWorker_ProgressChanged);
            syncBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(syncBackgroundWorker_RunWorkerCompleted);

            // Disable the start button
            btnStart.Enabled = false;

            // Initialize the progress bar and status textbox
            pbStatus.Value = 0;
            tbLastStatusMessage.Text = String.Empty;

            pictureBoxStatus.Visible = true;
            pictureBoxStatus.Enabled = true;

            // Kick off a background operation to synchronize
            syncBackgroundWorker.RunWorkerAsync();


        }


        private void syncBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Connect to the Subscriber and synchronize
            SynchronizeMergePullSubscriptionViaRMO();
        }

        public void SynchronizeMergePullSubscriptionViaRMO()
        {
            // Create a connection to the Subscriber.
            //ServerConnection conn = new ServerConnection(subscriberName);

            ServerConnection conn = new ServerConnection(subscriberName, srvLogin, srvPass);

            // Merge pull subscription
            MergeSubscription subscription;

            try
            {
                // Connect to the Subscriber.
                conn.Connect();

                // Define the pull subscription.
                subscription = new MergeSubscription();
                subscription.ConnectionContext = conn;
                subscription.DatabaseName = publicationDbName;
                subscription.PublicationName = publicationName;
                subscription.SubscriptionDBName = subscriptionDbName;
                subscription.SubscriberName = subscriberName;
                subscription.PublisherSecurity.SqlStandardLogin = srvLogin;
                subscription.PublisherSecurity.SqlStandardPassword = srvPass;

                // If the pull subscription exists, then start the synchronization.
                if (subscription.LoadProperties() && subscription.AgentJobId != null)
                {
                    
                    
                    // Get the agent for the subscription.
                    agent = subscription.SynchronizationAgent;

                    // Set the required properties that could not be returned
                    // from the MSsubscription_properties table.
                    agent.DistributorSecurityMode = SecurityMode.Standard;
                    agent.DistributorLogin = srvLogin;
                    agent.DistributorPassword = srvPass;

                    agent.PublisherSecurityMode = SecurityMode.Standard;
                    agent.PublisherLogin = srvLogin;
                    agent.PublisherPassword = srvPass;

                    // Enable verbose merge agent output to file.
                    agent.OutputVerboseLevel = 4;
                    agent.Output = "C:\\TEMP\\mergeagent.log";

                    // Handle the Status event
                    agent.Status += new AgentCore.StatusEventHandler(agent_Status);

                    // Synchronously start the Merge Agent for the subscription.
                    agent.Synchronize();
                }
                else
                {
                    // Do something here if the pull subscription does not exist.
                    throw new ApplicationException(String.Format(
                        "A subscription to '{0}' does not exist on {1}",
                        publicationName, subscriberName));
                }
            }
            catch (Exception ex)
            {
                // Implement appropriate error handling here.
                throw new ApplicationException("The subscription could not be " +
                    "synchronized. Verify that the subscription has " +
                    "been defined correctly.", ex);
            }
            finally
            {
                conn.Disconnect();
            }
        }



        // This event handler handles the Status event and reports the agent progress.
        public void agent_Status(object sender, StatusEventArgs e)
        {
            syncBackgroundWorker.ReportProgress(Convert.ToInt32(e.PercentCompleted), e.Message.ToString());
        }


        private void syncBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Set the progress bar percent completed
            pbStatus.Value = e.ProgressPercentage;

            // Append the last agent message
            tbLastStatusMessage.Text += e.UserState.ToString() + Environment.NewLine;

            // Scroll to end
            ScrollToEnd();
        }

        // This event handler deals with the results of the background operation.
        private void syncBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                tbLastStatusMessage.Text += "Canceled!" + Environment.NewLine;
                ScrollToEnd();
            }
            else if (e.Error != null)
            {
                tbLastStatusMessage.Text += "Error: " + e.Error.Message + Environment.NewLine;
                ScrollToEnd();
            }
            else
            {
                tbLastStatusMessage.Text += "Done!" + Environment.NewLine;
                ScrollToEnd();
            }

            btnStart.Enabled = true;
            pictureBoxStatus.Enabled = false;
        }


        private void ScrollToEnd()
        {
            // Scroll to end
            tbLastStatusMessage.SelectionStart = tbLastStatusMessage.TextLength;
            tbLastStatusMessage.ScrollToCaret();
        }



        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
