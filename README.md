# TCP/IP Communication Client and Server

This repository contains two Windows Forms applications designed for simple TCP/IP communication between a client and a server. These applications utilize the `SimpleTCP` library to facilitate easy setup and management of TCP connections, making it ideal for educational purposes, testing network protocols, or developing lightweight networking applications.

## Overview

### Client Application (`TCP_IP_Client`)
The client application allows users to establish a TCP connection to a specified host and port, send messages to the server, and receive responses. It includes functionalities such as:
- Connecting to a server using a specified IP address and port.
- Sending messages to the server and waiting for a reply.
- Receiving messages from the server and displaying them in real-time.
- Disconnecting from the server.

### Server Application (`TCP_IP_Server`)
The server application listens for incoming TCP connections, receives messages from clients, and sends back responses. It supports:
- Listening on a specified IP address and port.
- Automatically replying to every received message with a confirmation.
- Stopping the server gracefully.

## Getting Started

### Prerequisites
-.NET Framework 4.7.2 or later
- Visual Studio 2019 or newer

### Installation
1. Clone the repository to your local machine.
2. Open the solution in Visual Studio.
3. Build the solution to ensure there are no compilation errors.
4. Run either the `TCP_IP_Client` or `TCP_IP_Server` project depending on your role.

### Usage
- **For the Client:**
  - Enter the server's IP address and port in the corresponding fields.
  - Click the "Connect" button to initiate the connection.
  - Type a message in the text box and click "Send" to transmit it to the server.
  - View the conversation history in the status area.

- **For the Server:**
  - Specify the IP address and port to listen on.
  - Click "Start" to begin listening for incoming connections.
  - Messages sent by the client will appear in the status area, along with automatic replies.

## Author

**Tertius Denis Liebenberg**  

For any questions or feedback, please contact [tertiusliebenberg7@gmail.com].