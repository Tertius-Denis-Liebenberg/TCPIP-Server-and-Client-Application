using SimpleTCP;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

        private  void ServerForm_Load(object sender, EventArgs e)
        {
            server = new SimpleTcpServer();
            server.Delimiter = 0x13; // Enter
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

        private  void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;

            txtStatus.Text += "Starting server...";
            try
            {
                IPAddress ip;
                if (!IPAddress.TryParse(txtHost.Text, out ip))
                {
                    txtStatus.Text += "Invalid IP address.";
                    return;
                }

                int port;
                if (!int.TryParse(txtPort.Text, out port) || port < 1 || port > 65535)
                {
                    txtStatus.Text += "Invalid port number.";
                    return;
                }

                server.Start(ip, port);
                txtStatus.Text += "Server started successfully.";
            }
            catch (Exception ex)
            {
                txtStatus.Text += $"Failed to start server: {ex.Message}";
                btnStart.Enabled = true;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (server.IsStarted)
            {
                server.Stop();
                txtStatus.Text += "Server stopped.";
                btnStart.Enabled = true;
            }
            else
            {
                txtStatus.Text += "Server is not running.";
            }
        }
    }
}
