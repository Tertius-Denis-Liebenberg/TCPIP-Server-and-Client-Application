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
        // Declare a SimpleTcpClient instance to manage the client's operations
        SimpleTcpClient client;

        public ClientForm()
        {
            // Initialize the form components
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            // Disable the Connect button to prevent multiple attempts to connect
            btnConnect.Enabled = false;

            // Indicate that the connection attempt has started
            txtStatus.Text += "Attempting to connect...\r\n";

            try
            {
                // Attempt to connect to the server using the specified IP address and port
                client.Connect(txtHost.Text, Convert.ToInt32(txtPort.Text));
                // Indicate that the connection was successful
                txtStatus.Text += "Connected!\r\n";
            }
            catch (Exception ex)
            {
                // Handle any exceptions during the connection attempt
                txtStatus.Text += $"Connection failed: {ex.Message}\r\n";
            }
            finally
            {
                // Re-enable the Connect button regardless of the outcome
                btnConnect.Enabled = true;
            }
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            // Initialize the SimpleTcpClient with UTF8 encoding
            client = new SimpleTcpClient();
            client.StringEncoder = Encoding.UTF8;

            // Subscribe to the DataReceived event to handle incoming messages
            client.DataReceived += Client_DataRecieved;
        }

        private void Client_DataRecieved(object sender, SimpleTCP.Message e)
        {
            // Safely update the UI thread with the received message
            txtStatus.Invoke((MethodInvoker)delegate ()
            {

                // Replace '\u0013' with an empty string to remove it
                string cleanedMessage = e.MessageString.Replace("\u0013", "\r\n");

                txtStatus.AppendText(cleanedMessage + "\r\n"); // Append the received message to the status text
            });
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            // Check if the message text box is empty
            if (string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                // Inform the user to enter a message
                txtStatus.Text += "Please enter a message.\r\n";
                return;
            }

            // Send the message to the server and wait for a reply
            var reply = client.WriteLineAndGetReply(txtMessage.Text, TimeSpan.FromSeconds(3));

            // Update the status text with the sent message and the received reply
            txtStatus.Text += $"\r\nSent: {txtMessage.Text} \r\n";
            txtStatus.Text += $"Received: {reply} \r\n";
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            // Disconnect from the server
            client.Disconnect();

            // Indicate that the disconnection was successful
            txtStatus.Text += "Disconnected. \r\n";

            // Re-enable the Connect button
            btnConnect.Enabled = true;
        }
    }
}
