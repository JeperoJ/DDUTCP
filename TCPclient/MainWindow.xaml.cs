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
using System.Security.Cryptography;
using System.IO;

namespace TCPclient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Definerer variabler der skal være globale
        private static TcpClient client = new TcpClient(); //Klienten der håndterer at oprette forbindelse
        private static NetworkStream stream; //Streamen hvor informationen er
        //private static Thread thr; //Threaden hvor receive er
        private static Boolean open;
        private static CancellationTokenSource source;
        private static RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
        //private static ManualResetEvent mre = new ManualResetEvent(false); //Event til at stoppe receive funktionen
        public MainWindow()
        {
            InitializeComponent();
            this.Title = "Client Connect";
            RSAParameters RSAParam = RSA.ExportParameters(true);
            stackpanel.Children.Add(new TextBlock { Foreground = sentMessage.Foreground, FontSize = sentMessage.FontSize, Margin = sentMessage.Margin, Background = sentMessage.Background, TextWrapping = sentMessage.TextWrapping,  Text = Convert.ToBase64String(RSAParam.D) + " ;;;;;;;;; " + Convert.ToBase64String(RSAParam.Modulus) });
            string fileName = @"C:\Users\jeppe\Desktop\DDU Projekt\penis.txt";
            // Check if file already exists. If yes, delete it.

            // Create a new file     
            using (FileStream fs = File.Create(fileName))
            {
                // Add some text to file    
                Byte[] title = new UTF8Encoding(true).GetBytes(Convert.ToBase64String(RSAParam.D) + " ;;;;;;;;; " + Convert.ToBase64String(RSAParam.Modulus));
                fs.Write(title, 0, title.Length);
                byte[] author = new UTF8Encoding(true).GetBytes("\n\n" + String.Join(",", RSAParam.D.Select(p => p.ToString())));
                fs.Write(author, 0, author.Length);
            }



        }

        

        void Connection(object sender, RoutedEventArgs e)
        {
            try
            {
                client.Connect(ip.Text, Convert.ToInt32(port.Text)); //Forbinder til IP'en specificeret i tekst feltet.
                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();
                stream = client.GetStream(); //Får streamen fra den forbunde IP
                source = new CancellationTokenSource();
                open = true;
                Thread thr = new Thread(new ThreadStart(Receive)); //Binder receive funktionen til threaden
                thr.Start(); //Starter threaden

                connect.Content = "Disconnect";
                connect.Click -= Connection;
                connect.Click += Disconnect;

                /*RSAParameters RSAParam = RSA.ExportParameters(true);
                Byte[] data = Encoding.UTF8.GetBytes(Convert.ToBase64String(RSAParam.D) + " ;;;;;;;;; " + Convert.ToBase64String(RSAParam.Modulus));
                stream.Write(data, 0, data.Length);*/

            }
            catch (FormatException ex) //Fanger format fejl for input
            {
                MessageBox.Show("Forkert Inputformat. Check: Er IP i rigtigt format (punktum de rigtige steder) er port indtastet består kun af 4 tal?\n\nError msg:\n" + ex);
            }
            catch (SocketException ex)
            {
                MessageBox.Show("Kunne ikke forbinde til server. Kontrollér at rigtig IP og port er indtastet og at serveren er åben\n\nErrormsg:\n" + ex);
                //Hvis brugeren allerede er forbundet og forsøger at disconnecte, vil denne exception poppe op, hvilket vil sige at knappen ikke skifter ordentligt til disconnect.
            }
            catch (Exception ex) //Fanger generelle, uforudsete fejl
            {
                MessageBox.Show(ex.Message + "\n" + ex);
                //new ToastContentBuilder().AddArgument("action", "viewConversation").AddArgument("conversationId", 9813).AddText(Convert.ToString(excep)).Show();

            }
            
        }

        void Disconnect(object sender, RoutedEventArgs e)
        {
            try
            {
                //thr.Abort();
                open = false;
                source.Cancel();
                stream.Close(); //Lukker for informationen
                client.Close(); //Lukker for klienten
                stream.Dispose();
                client.Dispose();
                source.Dispose();
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
            //Byte[] data = Encoding.UTF8.GetBytes(sendMessage.Text); //Konverterer beskeden til bytes med UTF8 karakter sættet
            Byte[] encryptedData = { };
            try
            {
                encryptedData = RSAEncrypt(Encoding.UTF8.GetBytes(sendMessage.Text), RSA.ExportParameters(false), false);
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show("Enkryption Fejlede \n" + ex.Message + "\n" + ex);
            }
            
            // Send the message to the connected TcpServer.
            stream.Write(encryptedData, 0, encryptedData.Length); //Sender beskeden

            stackpanel.Children.Add(new TextBlock { Foreground = sentMessage.Foreground, FontSize = sentMessage.FontSize, Margin = sentMessage.Margin, Background = sentMessage.Background, TextWrapping = sentMessage.TextWrapping, Text = sendMessage.Text });
            stackpanel.Children.Add(new TextBlock { Foreground = sentMessage.Foreground, FontSize = sentMessage.FontSize, Margin = sentMessage.Margin, Background = sentMessage.Background, TextWrapping = sentMessage.TextWrapping, Text = String.Join(",", encryptedData.Select(p => p.ToString())) });
            string fileName = @"C:\Users\jeppe\Desktop\DDU Projekt\penisw.txt";
            // Check if file already exists. If yes, delete it.

            // Create a new file     
            using (FileStream fs = File.Create(fileName))
            {
                // Add some text to file    
                Byte[] title = new UTF8Encoding(true).GetBytes(String.Join(",", encryptedData.Select(p => p.ToString())));
                fs.Write(title, 0, title.Length);
                byte[] author = new UTF8Encoding(true).GetBytes("Mahesh Chand");
                fs.Write(author, 0, author.Length);
            }

            sendMessage.Text = "";

            //mre.Set(); //Starter receive threaden igen
        }

        void Receive()
        {
            CancellationToken token = source.Token;
            while (!token.IsCancellationRequested)
            {
                //MainWindow window = new MainWindow();
                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                Byte[] data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                try
                {
                    Task<int> bytes = stream.ReadAsync(data, 0, data.Length, token);
                    responseData = Encoding.UTF8.GetString(data, 0, bytes.Result);
                }
                catch (System.IO.IOException ex) { MessageBox.Show("Blocking operation blev afbrudt af et WSACancelBlocking Call\n\nError msg:\n" + ex); }
                catch (ObjectDisposedException ex) { MessageBox.Show("Det bliver forsøgt at skrive til en lukket stream\n\nError msg:\n" + ex); }
                catch (Exception ex) {MessageBox.Show("Ikke forberedet på exception: " + ex.Message + "\n\nError msg:\n" + ex); }
                Dispatcher.Invoke(new Action(() => { stackpanel.Children.Add(new TextBlock { Foreground = receiveMessage.Foreground, FontSize = receiveMessage.FontSize, Margin = receiveMessage.Margin, Background = receiveMessage.Background, TextWrapping = receiveMessage.TextWrapping, Text = responseData }); }));
                //mre.WaitOne();
            }
            
        }

        public static byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {

                    //Import the RSA Key information. This only needs
                    //toinclude the public key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Encrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.  
                    encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
                }
                return encryptedData;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }

        public static byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Import the RSA Key information. This needs
                    //to include the private key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Decrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.  
                    decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
                }
                return decryptedData;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }
        }
    }
}
