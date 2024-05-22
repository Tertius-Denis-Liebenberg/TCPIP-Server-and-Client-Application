using SimpleTCP;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace TCP_IP_Client
{
    public partial class ClientForm : Form
    {
        public ClientForm()
        {
            InitializeComponent();
        }

        SimpleTcpClient client;

        private void btnConnect_Click(object sender, EventArgs e)
        {
            btnConnect.Enabled = false;
            txtStatus.Text += "Attempting to connect...\n";

            try
            {
                client.Connect(txtHost.Text, Convert.ToInt32(txtPort.Text));
                txtStatus.Text += "Connected!\n";
            }
            catch (Exception ex)
            {
                txtStatus.Text += $"Connection failed: {ex.Message}\n";
            }
            finally
            {
                btnConnect.Enabled = true;
            }
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            client = new SimpleTcpClient();
            client.StringEncoder = Encoding.UTF8;
            client.DataReceived += Client_DataRecieved;
        }

        private void Client_DataRecieved(object sender, SimpleTCP.Message e)
        {
            txtStatus.Invoke((MethodInvoker)delegate ()
            {
                txtStatus.AppendText(e.MessageString + "\n");
            });
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                txtStatus.Text += "Please enter a message.\n";
                return;
            }

            var reply = client.WriteLineAndGetReply(txtMessage.Text, TimeSpan.FromSeconds(3));
            txtStatus.Text += $"Sent: {txtMessage.Text}\n";
            txtStatus.Text += $"Received: {reply}\n";
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            client.Disconnect();
            txtStatus.Text += "Disconnected.\n";
            btnConnect.Enabled = true;
        }
    }
}