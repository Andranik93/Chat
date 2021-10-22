using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerClientChat
{
    public partial class Server : Form
    {
        TcpListener _server;
        TcpClient _client;
        NetworkStream _netStream;
        Thread _thread;
        Boolean _accepting = false;
        public Server()
        {
            InitializeComponent();
            _client = new TcpClient();
        }

        private void Server_Load(object sender, EventArgs e)
        {
            IPHostEntry _host = Dns.GetHostEntry(Dns.GetHostName());
            this.txtIPAddress.Text = _host.AddressList[0].ToString();
            this.txtHostName.Text = _host.HostName.ToString();
            this.txtPort.Text = "11100";
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            _server = new TcpListener(IPAddress.Parse(this.txtIPAddress.Text), Convert.ToInt32(this.txtPort.Text));
            _server.Start();
            this.StartButton.Enabled = false;
            this.StopButton.Enabled = true;
            _thread = new Thread(AcceptClientSystem);
            _thread.Start();
            _accepting = true;
        }

        private void AcceptClientSystem()
        {
            _client = _server.AcceptTcpClient();
            _netStream = _client.GetStream();


            while (_accepting)
            {

                if (_netStream.DataAvailable)
                {
                    byte[] msg = new byte[256];
                    _netStream.Read(msg, 0, msg.Length);
                    string _msg = Encoding.ASCII.GetString(msg);
                    
                    listMessage.Invoke(new Action<string>(ShowMessage), _msg);
                }                
            }
        }
        private void ShowMessage(string message)
        {
            this.listMessage.Text += $"{message}\n";
        }
        private void StopButton_Click(object sender, EventArgs e)
        {
            _server.Stop();
        }
    }
}
