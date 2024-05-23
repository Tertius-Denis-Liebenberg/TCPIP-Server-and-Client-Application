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
using System.Net.NetworkInformation;

namespace TCP_IP_Server
{
    public partial class ServerForm : Form
    {
        // Declare a SimpleTcpServer instance to manage the server's operations
        SimpleTcpServer server;

        public ServerForm()
        {
            // Initialize the form components
            InitializeComponent();
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            GetWiFiIPAddress(txtHost);

            // Initialize the SimpleTcpServer with UTF8 encoding and set the delimiter to Enter key
            server = new SimpleTcpServer();
            server.Delimiter = 0x13; // ASCII value for Enter key
            server.StringEncoder = Encoding.UTF8;

            // Subscribe to the DataReceived event to handle incoming messages
            server.DataReceived += Server_DataRecieved;
        }

        private void Server_DataRecieved(object sender, SimpleTCP.Message e)
        {
            // Safely update the UI thread with the received message and send a reply
            txtStatus.Invoke((MethodInvoker)delegate ()
            {
                // Replace '\u0013' with an empty string to remove it
                string cleanedMessage = e.MessageString.Replace("\u0013", "");

                // Assign the cleaned message to txtStatus.Text
                txtStatus.AppendText(cleanedMessage);

                // Optionally, reply to the client
                e.ReplyLine(string.Format("Data Received: {0}", e.MessageString));
            });
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            // Disable the Start button to prevent multiple attempts to start the server
            btnStart.Enabled = false;

            // Update the status text to indicate the server is starting
            txtStatus.Text += "Starting server...";

            try
            {
                // Parse the IP address from the text box
                IPAddress ip;
                if (!IPAddress.TryParse(txtHost.Text, out ip))
                {
                    // If the IP address is invalid, display an error message and exit the method
                    txtStatus.Text += "Invalid IP address.";
                    return;
                }

                // Parse the port number from the text box
                int port;
                if (!int.TryParse(txtPort.Text, out port) || port < 1 || port > 65535)
                {
                    // If the port number is invalid, display an error message and exit the method
                    txtStatus.Text += "Invalid port number.";
                    return;
                }

                // Start the server with the parsed IP address and port
                server.Start(ip, port);
                // Update the status text to indicate the server has started successfully
                txtStatus.Text += "Server started successfully.";
            }
            catch (Exception ex)
            {
                // If an exception occurs, display the error message and re-enable the Start button
                txtStatus.Text += $"Failed to start server: {ex.Message}";
                btnStart.Enabled = true;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            // Check if the server is currently running
            if (server.IsStarted)
            {
                // Stop the server and update the status text
                server.Stop();
                txtStatus.Text += "Server stopped.";

                // Re-enable the Start button
                btnStart.Enabled = true;
            }
            else
            {
                // If the server is not running, display a message
                txtStatus.Text += "Server is not running.";
            }
        }
        
        // Method to retrieve the first non-link-local/non-multicast IP address of all Ethernet network interfaces
        private void GetLocalIPAddresses(TextBox textBox)
        {
            // Loop through all network interfaces on the machine
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Check if the current network interface is of type Ethernet
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    // Loop through all unicast IP addresses assigned to the Ethernet interface
                    foreach (UnicastIPAddressInformation ip in nic.GetIPProperties().UnicastAddresses)
                    {
                        // Check if the IP address is not link-local or multicast
                        if (!ip.Address.IsIPv6LinkLocal && !ip.Address.IsIPv6Multicast)
                        {
                            // Update the TextBox with the IP address
                            textBox.Text = ip.Address.ToString();
                            // Exit the loop after finding the first suitable IP address
                            break;
                        }
                    }
                }
            }
        }

        // Method to retrieve the first non-link-local/non-multicast IP address of the specified WiFi network interface
        private void GetWiFiIPAddress(TextBox textBox)
        {
            // Define the name of the WiFi adapter
            string wifiAdapterName = "Wi-Fi";

            // Try to find the network interface by its name
            NetworkInterface wifiNic = null;
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // If the network interface name matches the WiFi adapter name
                if (nic.Name.Equals(wifiAdapterName, StringComparison.OrdinalIgnoreCase))
                {
                    // Store the WiFi network interface for further processing
                    wifiNic = nic;
                    // Exit the loop once the WiFi adapter is found
                    break;
                }
            }

            // Check if the WiFi network interface was found
            if (wifiNic != null)
            {
                // Get the IP properties of the WiFi adapter
                IPInterfaceProperties wifiIpProps = wifiNic.GetIPProperties();

                // Loop through all unicast IP addresses assigned to the WiFi interface
                foreach (UnicastIPAddressInformation ip in wifiIpProps.UnicastAddresses)
                {
                    // Check if the IP address is not link-local or multicast
                    if (!ip.Address.IsIPv6LinkLocal && !ip.Address.IsIPv6Multicast)
                    {
                        // Update the TextBox with the IP address
                        textBox.Text = ip.Address.ToString();
                        // Exit the loop after finding the first suitable IP address
                        break;
                    }
                }
            }
            else
            {
                // Inform the user if the WiFi adapter couldn't be found
                textBox.Text = "WiFi adapter not found.";
            }
        }
    }
}
