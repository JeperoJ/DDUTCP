using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Net.Sockets;
using System.Net;
//using Microsoft.Toolkit.Uwp.Notifications;

namespace TCPclient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public TcpClient client = new TcpClient();

        void Connection(object sender, RoutedEventArgs e)
        {
            try
            {
                client.Connect(ip.Text, Convert.ToInt32(port.Text));
            }
            catch //(SocketException excep)
            {
                //new ToastContentBuilder().AddArgument("action", "viewConversation").AddArgument("conversationId", 9813).AddText(Convert.ToString(excep)).Show();

            }
        }

        void Send(object sender, RoutedEventArgs e)
        {
            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] data = Encoding.ASCII.GetBytes(sendMessage.Text);

            // Get a client stream for reading and writing.
            //  Stream stream = client.GetStream();

            NetworkStream stream = client.GetStream();

            // Send the message to the connected TcpServer.
            stream.Write(data, 0, data.Length);

            sentMessage.Content = sendMessage.Text;

            sendMessage.Text = "";
        }
    }
}
