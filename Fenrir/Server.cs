using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using Sleipnir;
using System.Net.NetworkInformation;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

//Released as open source by NCC Group Plc - http://www.nccgroup.com/
//
//Developed by Dave Spencer, david [dot] spencer [at] nccgroup [dot] com
//
//http://www.github.com/nccgroup/Fenrir
//
//Released under AGPL see LICENSE for more information


namespace Fenrir
{
    public class Server
    {
        private TcpListener tcpListener;
        private Thread listenThread;
        public IntPtr handle = WTSapi32.WTSVirtualChannelOpen(IntPtr.Zero, -1, "Fenrir");
        public string sFullResponse = "";

        public Server(int iPort)
        {
            bool isAvailable = true;
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();
            IPEndPoint[] tcpListInfoArray = ipGlobalProperties.GetActiveTcpListeners();

            foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
            {
                if (tcpi.LocalEndPoint.Port == iPort)
                {
                    isAvailable = false;
                    break;
                }
            }


            foreach (IPEndPoint tcpi in tcpListInfoArray)
            {
                if (tcpi.Port == iPort)
                {
                    isAvailable = false;
                    break;
                }
            }

            if (isAvailable == true)
            {
                this.tcpListener = new TcpListener(IPAddress.Loopback, iPort);
                this.listenThread = new Thread(new ThreadStart(ListenForClients));
                this.listenThread.IsBackground = true;
                this.listenThread.Start();
            }
            else
            {
                MessageBox.Show("Port already in use, choose another");
            }
        }

        public void closePort()
        {
            this.tcpListener.Stop();
        }


        private void ListenForClients()
        {
            this.tcpListener.Start();
           
            while (true)
            {
                //blocks until a client has connected to the server
                TcpClient client = this.tcpListener.AcceptTcpClient();

                //create a thread to handle communication
                //with connected client
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                Thread clientThreadSend = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.Start(client);
                clientThread.IsBackground = true;
                clientThreadSend.Start(client);
                clientThreadSend.IsBackground = true;
            }
        }
        static readonly object _locker = new object();
        private void HandleClientComm(object client)
        {
            //Set a really long time out so that the meterpreter shell can initiate ok
            TcpClient tcpClient = (TcpClient)client;
            tcpClient.ReceiveTimeout = 1600000;
            TcpClient sendClient = (TcpClient)client;
            sendClient.ReceiveTimeout = 1600000;
            NetworkStream clientStream = tcpClient.GetStream();


            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    //a socket error has occured
                    break;
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    break;
                }

                //message has successfully been received
                ASCIIEncoding encoder = new ASCIIEncoding();
                System.Diagnostics.Debug.WriteLine(encoder.GetString(message, 0, bytesRead));
                

                Console.WriteLine(encoder.GetString(message, 0, bytesRead));
                clientStream.Flush();
                
                //Lock it so that multiple threads dont write to the virtual channel at the same time
                lock (_locker)
                {
                    sendToVC(Convert.ToBase64String(message, 0, bytesRead), handle);
                }    
                    // Append request to an existing file. 
                    // The using statement automatically closes the stream and calls  
                    // IDisposable.Dispose on the stream object. 
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"Fenrir.log", true))
                    {
                        file.WriteLine("Request " + DateTime.Now.ToString("dd/MM/yyyy h:mm:ss tt"));
                        file.Write(encoder.GetString(message, 0, bytesRead));
                    }

                    sendToClient(sFullResponse, sendClient);      
                              
            }

            tcpClient.Close();
            
        }


        public void sendToClient(String line, TcpClient client)
        {
            try
            {
                MemoryStream memStream = new MemoryStream();
                NetworkStream stream = client.GetStream();
                ASCIIEncoding encoder = new ASCIIEncoding();
                string[] split = line.Split(new Char[] {'$'});
                //need to iterate through each entry in the string array, base64 decode and combine in to one large byte array
                foreach (string entry in split)
                {
                    byte[] decoded;
                    decoded = Convert.FromBase64String(entry);
                    foreach (byte b in decoded)
                    {
                        memStream.WriteByte(b);
                    }

                }
                byte[] buffer = new byte[memStream.Length];
                buffer = memStream.ToArray();
          
                stream.Write(buffer, 0, buffer.Length);

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"Fenrir.log", true))
                {
                    file.WriteLine("Response " + DateTime.Now.ToString("dd/MM/yyyy h:mm:ss tt"));
                    file.Write(encoder.GetString(buffer, 0, buffer.Length) + "\r\n");
                }

                stream.Flush();
                stream.Close();
            }
            catch
            {
                Console.WriteLine("Client Disconnected."+Environment.NewLine);
            }
           
        }

        public void sendToVC(String line, IntPtr handle)
        {
            byte[] bBeginning = System.Text.Encoding.Unicode.GetBytes("Start of Request");
            int bytesforBeginning = 0;

            WTSapi32.WTSVirtualChannelWrite(handle, bBeginning, bBeginning.Length, ref bytesforBeginning);
            int incomingOffset = 0;
            byte[] bData = System.Text.Encoding.Unicode.GetBytes(line);
            byte[] bChunk = new byte[1024];

            while (incomingOffset < bData.Length)
            {
                int length = Math.Min(bChunk.Length, bData.Length - incomingOffset);

               Buffer.BlockCopy(bData, incomingOffset, bChunk, 0, length);

                incomingOffset += length;

                // Transmit outbound buffer
                try
                {
                    //neeed to split this down to less than 1.5k 
                    byte[] title;
                    read(out title);
                    string sReceived = "";
                    foreach (byte b in title)
                    {
                        if (b != 0)
                        {
                            char c = Convert.ToChar(b);
                            sReceived += c;
                        }
                        else
                        {
                        }
                    }
                    if (sReceived == "Received")
                    {
                        int bytesWritten = 0;
                        
                        WTSapi32.WTSVirtualChannelWrite(handle, bChunk, bChunk.Length, ref bytesWritten);
                        Array.Clear(bChunk, 0, bChunk.Length);
                    }

                }
                catch
                {
                    Console.WriteLine("Error" + Environment.NewLine);

                }
            }
            byte[] bTerminator = System.Text.Encoding.Unicode.GetBytes("End of Request");
            int bytes = 0;
            
            WTSapi32.WTSVirtualChannelWrite(handle, bTerminator, bTerminator.Length, ref bytes);
            
            //The read section to retrieve the response is below here!!!!!!!

            sFullResponse = "";
            

            while (true)
            {
                try
                {
                    byte[] bResponse;
                    read(out bResponse);
                    string sResponse = "";
                    
                    foreach (byte b in bResponse)
                    {
                        if (b != 0)
                        {
                            char c = Convert.ToChar(b);
                            sResponse += c;
                        }
                        else
                        {
                        }
                    }
                    if (sResponse == "End of Response")
                    {
                        break;
                    }
                    else if (sResponse == "Received")
                    {
                        //do nothing
                    }
                    else
                    {
                        sFullResponse += sResponse;
                    }

                    

                }
                catch
                {
                    Console.WriteLine("Error" + Environment.NewLine);

                }
            }

        }

        // Create a handle for writing to
        public IntPtr handleFenrir = WTSapi32.WTSVirtualChannelOpen(IntPtr.Zero, -1, "Fenrir");

        // The read method, reads data from the virtual channel
        private void read(out byte[] data)
        {
            byte[] readInData = new byte[1536];
            GCHandle pinned = GCHandle.Alloc(readInData, GCHandleType.Pinned);
            IntPtr address = pinned.AddrOfPinnedObject();

            uint bytesread = 0;
            int returnValue = WTSapi32.WTSVirtualChannelRead(handleFenrir, int.MaxValue, readInData, 1536, out bytesread);
            pinned.Free();

            byte[] correct = new byte[bytesread];
            Array.Copy(readInData, correct, bytesread);
            data = correct;
            Array.Clear(readInData, 0, readInData.Length);
            Application.DoEvents();
        }

    }
}
