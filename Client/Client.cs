using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Client
{
    public partial class Client : Form
    {
        TcpClient _client;
        NetworkStream _nwStream;
        Thread _thread;
        Boolean _listen = false;

        public Client()
        {
            InitializeComponent();
        }

        private void Client_Load(object sender, EventArgs e)
        {

        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {

            if (this.txtRemoteIP.Text == ""|| this.txtRemoteHost.Text == ""|| this.txtRemotePort.Text == "")
                return;

            _client = new TcpClient();
            _client.Connect(IPAddress.Parse(this.txtRemoteIP.Text), Convert.ToInt32(this.txtRemotePort.Text));
            _nwStream = _client.GetStream();

            if (_client.Connected)
            {

                _thread = new Thread(Listening);
                _thread.Start();

                //var nwStream = _client.GetStream();
                //byte[] msg = Encoding.ASCII.GetBytes("Hi Server");
                //nwStream.Write(msg, 0, msg.Length);

                _listen = true;
            }
        }

        private void Listening()
        {
            while (_listen)
            {
                if (_nwStream.DataAvailable)
                {
                    byte[] msg = new byte[256];
                    _nwStream.Read(msg, 0, msg.Length);
                    string _msg = Encoding.ASCII.GetString(msg);
                    this.listMessage.Text += _msg + '\n'; 
                }
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            if (_client.Connected)
            {
                var nwStream = _client.GetStream();
                byte[] msg = Encoding.ASCII.GetBytes(this.txtSend.Text+'\n');
                nwStream.Write(msg, 0, msg.Length);
                //_client.Client.Send(msg);
            }
        }
    }
}
