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

            try
            {
                
            }
            catch { }

            /*while (true)
            {
                
            }*/
        }

        private static TcpClient client = new TcpClient();
        private static NetworkStream stream;
        private static Thread thr;

        void Connection(object sender, RoutedEventArgs e)
        {
            try
            {
                client.Connect(ip.Text, Convert.ToInt32(port.Text));
                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                stream = client.GetStream();

                thr = new Thread(new ThreadStart(Receive));
                thr.Start();


            }
            catch //(SocketException excep)
            {
                //new ToastContentBuilder().AddArgument("action", "viewConversation").AddArgument("conversationId", 9813).AddText(Convert.ToString(excep)).Show();

            }
            finally
            {
                connect.Content = "Disconnect";
                connect.Click += Disconnect;
            }
        }

        void Disconnect(object sender, RoutedEventArgs e)
        {
            try
            {
                stream.Close();
                client.Close();
            }
            catch //(SocketException excep)
            {
                //new ToastContentBuilder().AddArgument("action", "viewConversation").AddArgument("conversationId", 9813).AddText(Convert.ToString(excep)).Show();

            }
            finally
            {
                connect.Content = "Connect";
                connect.Click += Connection;
            }
        }

        void Send(object sender, RoutedEventArgs e)
        {
            //thr.Abort();
            
            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] data = Encoding.ASCII.GetBytes(sendMessage.Text);
            // Send the message to the connected TcpServer.
            stream.Write(data, 0, data.Length);

            sentMessage.Content = sendMessage.Text;

            sendMessage.Text = "";
            //thr.Start();
        }

        void Receive()
        {
            //MainWindow window = new MainWindow();
            // Receive the TcpServer.response.

            // Buffer to store the response bytes.
            Byte[] data = new Byte[256];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = Encoding.ASCII.GetString(data, 0, bytes);
            receiveMessage.Content = responseData;
           
            
        }
    }
}
