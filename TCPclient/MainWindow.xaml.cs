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
            

            /*try
            {
                
            }
            catch { }*/

            // Receive the TcpServer.response.


        }

        //Definerer variabler der skal være globale
        private static TcpClient client = new TcpClient(); //Klienten der håndterer at oprette forbindelse
        private static NetworkStream stream; //Streamen hvor informationen er
        private static Thread thr; //Threaden hvor receive er
        //private static ManualResetEvent mre = new ManualResetEvent(false); //Event til at stoppe receive funktionen

        void Connection(object sender, RoutedEventArgs e)
        {
            try
            {
                client.Connect(ip.Text, Convert.ToInt32(port.Text)); //Forbinder til IP'en specificeret i tekst feltet.
                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();
                stream = client.GetStream(); //Får streamen fra den forbunde IP
                thr = new Thread(new ThreadStart(Receive)); //Binder receive funktionen til threaden
                thr.Start(); //Starter threaden


            }
            catch //(SocketException excep)
            {
                //new ToastContentBuilder().AddArgument("action", "viewConversation").AddArgument("conversationId", 9813).AddText(Convert.ToString(excep)).Show();

            }
            finally
            {
                //Skifter knappen der forbandt til en knap der kan lukke forbindelsem
                connect.Content = "Disconnect";
                connect.Click += Disconnect;
            }
        }

        void Disconnect(object sender, RoutedEventArgs e)
        {
            try
            {
                //thr.Abort();
                //thr.Join();
                stream.Close(); //Lukker for informationen
                client.Close(); //Lukker for klienten
            }
            catch //(SocketException excep)
            {
                //new ToastContentBuilder().AddArgument("action", "viewConversation").AddArgument("conversationId", 9813).AddText(Convert.ToString(excep)).Show();

            }
            finally
            {
                //Sætter knappen til at forbinde igen
                connect.Content = "Connect";
                connect.Click += Connection;
            }
        }

        void Send(object sender, RoutedEventArgs e)
        {
            //mre.Reset(); //Slukker for receive threaden
            
            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] data = Encoding.UTF8.GetBytes(sendMessage.Text); //Konverterer beskeden til bytes med UTF8 karakter sættet
            // Send the message to the connected TcpServer.
            stream.Write(data, 0, data.Length); //Sender beskeden

            stackpanel.Children.Add(new Label { Foreground = sentMessage.Foreground, FontSize = sentMessage.FontSize, Margin = sentMessage.Margin, Background = sentMessage.Background, Content = sendMessage.Text });
            sendMessage.Text = "";

            //mre.Set(); //Starter receive threaden igen
        }

        void Receive()
        {
            while (true)
            {
                //MainWindow window = new MainWindow();
                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                Byte[] data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                int bytes = stream.Read(data, 0, data.Length);
                responseData = Encoding.UTF8.GetString(data, 0, bytes);
                Dispatcher.Invoke(new Action(() => { stackpanel.Children.Add(new Label { Foreground = receiveMessage.Foreground, FontSize = receiveMessage.FontSize, Margin = receiveMessage.Margin, Background = receiveMessage.Background, Content = responseData }); }));
                //mre.WaitOne();
            }
            
        }

        
    }
}
