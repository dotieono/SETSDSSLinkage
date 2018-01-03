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
    public partial class Synchronize : Form
    {

        string publicationName = Convert.ToString(ConfigurationManager.AppSettings["PublicationName"]);
        string publisherName = Convert.ToString(ConfigurationManager.AppSettings["PublisherName"]);
        string subscriberName = Convert.ToString(ConfigurationManager.AppSettings["SubscriberName"]);
        string publicationDbName = Convert.ToString(ConfigurationManager.AppSettings["PublicationDbName"]);
        string subscriptionDbName = Convert.ToString(ConfigurationManager.AppSettings["SubscriptionDbName"]);
        string IpAddress = Convert.ToString(ConfigurationManager.AppSettings["PublisherIP"]);
        string srvLogin = Convert.ToString(ConfigurationManager.AppSettings["PublisherLogin"]);
        string srvPass = Convert.ToString(ConfigurationManager.AppSettings["PublisherPass"]);
        public string syn_category = "";

        public Synchronize(string synchronization_type)
        {
            InitializeComponent();
            syn_category = synchronization_type;

        }


        private void CheckPublisherConnection()
        {


            string returnMessage = string.Empty;

            //retrieving the Ip address from the supplied hostname


            IPHostEntry host = Dns.GetHostEntry(publisherName.Trim());
            IPAddress[] ipaddr = host.AddressList;


            foreach (IPAddress addr in ipaddr)
            {
                IpAddress = addr.ToString();
            }



            // Loop through the IP Address array and add the IP address to Listbox



            string urlOrIp = IpAddress;
            PingOptions pingOptions = new PingOptions(128, true);


            textBoxConnectivity.Text = "Checking connectivity to " + urlOrIp;
            Ping ping = new Ping();
            byte[] buffer = new byte[32];

            for (int i = 0; i < 4; i++)
            {
                try
                {
                    PingReply pingReply = ping.Send(IPAddress.Parse(urlOrIp), 100, buffer, pingOptions);
                    if (pingReply != null)
                    {
                        switch (pingReply.Status)
                        {
                            case IPStatus.Success:
                                returnMessage = string.Format("Reply from {0}: bytes={1} time={2}ms TTL={3}", pingReply.Address, pingReply.Buffer.Length, pingReply.RoundtripTime, pingReply.Options.Ttl);
                                pictureBoxConnected.Visible = true;
                                pictureBoxNoConnection.Visible = false;
                                btnPushData.Enabled = true;

                                break;
                            case IPStatus.TimedOut:
                                returnMessage = "Connection has timed out...";
                                pictureBoxConnected.Visible = false;
                                pictureBoxNoConnection.Visible = true;
                                break;
                            default:
                                returnMessage = string.Format("Ping failed: {0}", pingReply.Status.ToString());
                                pictureBoxConnected.Visible = false;
                                pictureBoxNoConnection.Visible = true;
                                break;

                        }
                    }
                    else
                    {
                        returnMessage = "Connection failed for an unknown reason...";
                    }
                }
                catch (PingException ex)
                {
                    returnMessage = string.Format("Connection Error: {0}", ex.Message);
                }
                catch (SocketException ex)
                {
                    returnMessage = string.Format("Connection Error: {0}", ex.Message);
                }
            }

            textBoxConnectivity.Text = returnMessage;




        }


        private void CheckSubscriberConnection()
        {

            string returnMessage = string.Empty;

            //retrieving the Ip address from the supplied hostname


            IPHostEntry host = Dns.GetHostEntry(subscriberName.Trim());
            IPAddress[] ipaddr = host.AddressList;


            foreach (IPAddress addr in ipaddr)
            {
                IpAddress = addr.ToString();
            }



            // Loop through the IP Address array and add the IP address to Listbox



            string urlOrIp = IpAddress;
            PingOptions pingOptions = new PingOptions(128, true);


            textBoxConnectivity.Text = "Checking connectivity to " + urlOrIp;
            Ping ping = new Ping();
            byte[] buffer = new byte[32];

            for (int i = 0; i < 4; i++)
            {
                try
                {
                    PingReply pingReply = ping.Send(IPAddress.Parse(urlOrIp), 100, buffer, pingOptions);
                    if (pingReply != null)
                    {
                        switch (pingReply.Status)
                        {
                            case IPStatus.Success:
                                returnMessage = string.Format("Reply from {0}: bytes={1} time={2}ms TTL={3}", pingReply.Address, pingReply.Buffer.Length, pingReply.RoundtripTime, pingReply.Options.Ttl);
                                pictureBoxConnected.Visible = true;
                                pictureBoxNoConnection.Visible = false;
                                btnPushData.Enabled = true;

                                break;
                            case IPStatus.TimedOut:
                                returnMessage = "Connection has timed out...";
                                pictureBoxConnected.Visible = false;
                                pictureBoxNoConnection.Visible = true;
                                break;
                            default:
                                returnMessage = string.Format("Ping failed: {0}", pingReply.Status.ToString());
                                pictureBoxConnected.Visible = false;
                                pictureBoxNoConnection.Visible = true;
                                break;

                        }
                    }
                    else
                    {
                        returnMessage = "Connection failed for an unknown reason...";
                    }
                }
                catch (PingException ex)
                {
                    returnMessage = string.Format("Connection Error: {0}", ex.Message);
                }
                catch (SocketException ex)
                {
                    returnMessage = string.Format("Connection Error: {0}", ex.Message);
                }
            }

            textBoxConnectivity.Text = returnMessage;




        }
		






        private void buttonCheckConnectivity_Click(object sender, EventArgs e)
        {

            if (syn_category == "push") { CheckSubscriberConnection(); }

            else if (syn_category == "pull") { CheckPublisherConnection(); }


        }

        private void btnPushData_Click(object sender, EventArgs e)
        {

            ServerConnection conn = new ServerConnection(publisherName, srvLogin, srvPass);

            MergeSubscription subscription;

            try
            {
                // Connect to the Publisher.
                conn.Connect();
                //conn.Login = srvLogin;
                //conn.Password = srvPass;
                

                // Define push subscription.
                subscription = new MergeSubscription();
                subscription.ConnectionContext = conn;
                subscription.DatabaseName = publicationDbName;
                subscription.PublicationName = publicationName;
                subscription.SubscriptionDBName = subscriptionDbName;
                subscription.SubscriberName = subscriberName;
                subscription.PublisherSecurity.SqlStandardLogin = srvLogin;
                subscription.PublisherSecurity.SqlStandardPassword = srvPass;
                

                // If the push subscription and the job exists, start the agent job.
                if (subscription.LoadProperties() && subscription.AgentJobId != null)
                {
                    // Start the Merge Agent asynchronously.
                    //subscription.SynchronizeWithJob();
                    subscription.SynchronizationAgent.DistributorSecurityMode = SecurityMode.Standard;
                    subscription.SynchronizationAgent.DistributorLogin = srvLogin;
                    subscription.SynchronizationAgent.DistributorPassword = srvPass;

                    subscription.SynchronizationAgent.PublisherSecurityMode = SecurityMode.Standard;
                    subscription.SynchronizationAgent.PublisherLogin = srvLogin;
                    subscription.SynchronizationAgent.PublisherPassword = srvPass;

                    subscription.SynchronizationAgent.Synchronize();



                    //Create this message box in case data exchange is successful

                    MessageBox.Show(" " + subscriberName + "  successfully exchanged data with " + publisherName + " ", "SETS: System Data Exchange Window", MessageBoxButtons.OKCancel,
MessageBoxIcon.Information);

                }


                else
                {
                    // Do something here if the subscription does not exist.
                    throw new ApplicationException(String.Format(
                        "A subscription to '{0}' does not exists on {1}",
                        publicationName, subscriberName));
                }
            }
            catch (Exception ex)
            {
                // Implement appropriate error handling here.
                throw new ApplicationException("The subscription could not be synchronized.", ex);
            }
            finally
            {
                conn.Disconnect();
            }

        }

        private void btnCloseSync_Click(object sender, EventArgs e)
        {
            this.Close();
        }





    }
}
