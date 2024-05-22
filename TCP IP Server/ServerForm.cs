using SimpleTCP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace TCP_IP_Server
{
    public partial class ServerForm : Form
    {
        public ServerForm()
        {
            InitializeComponent();
        }

        SimpleTcpServer server;

        private void ServerForm_Load(object sender, EventArgs e)
        {
            server = new SimpleTcpServer();
            server.Delimiter = 0x13; //Enter
            server.StringEncoder = Encoding.UTF8;
            server.DataReceived += Server_DataRecieved;
        }

        private void Server_DataRecieved(object sender, SimpleTCP.Message e)
        {
            txtStatus.Invoke((MethodInvoker)delegate ()
            {
                txtStatus.Text += "\n" + e.MessageString;
                e.ReplyLine(string.Format("Data Recieved: {0}", e.MessageString));
            });
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            txtStatus.Text += "Server starting...";
            // Correctly parse the IP address string
            IPAddress ip;
            if (!IPAddress.TryParse(txtHost.Text, out ip))
            {
                txtStatus.Text += "Invalid IP address.";
                return; // Exit the method if the IP address is invalid
            }
            server.Start(ip, Convert.ToInt32(txtPort.Text));
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (server.IsStarted)
            {
                server.Stop();
            }
        }
    }
}
