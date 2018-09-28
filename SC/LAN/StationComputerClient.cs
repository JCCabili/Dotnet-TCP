using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SC.LAN
{
    public class StationComputerClient : StationComputerBase
    {
        TcpListener server = null;

        private byte[] bytes;
        private string data;

        public eState eStateResponse { get; set; }

        public StationComputerClient()
        {
            bytes = new Byte[256];
            data = string.Empty;
        }

        public bool Open()
        {
            Int32 port = 8008;

            
            if (PingAddress(Global.IPaddress))
            {
                IPAddress localAddress = IPAddress.Parse(Global.IPaddress);
                server = new TcpListener(localAddress, port);
                server.Start();
                return true;
            }
            else
                return false;

        }
        
        

        public bool AcceptClient()
        {
            if (server.Pending())
            {
                TcpClient client = server.AcceptTcpClient();
                data = null;

                NetworkStream stream = client.GetStream();

                int i;

                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = BitConverter.ToString(bytes, 0, i);
                    Debug.WriteLine("Recieved:" + data);
                    stream.Write(Constants.RECEIVE,0, Constants.RECEIVE.Length);



                    if (data == BitConverter.ToString(Constants.INSERVICE, 0, Constants.INSERVICE.Length))
                        eStateResponse = eState.InService;
                    else if (data == BitConverter.ToString(Constants.MAINTENANCE,0, Constants.MAINTENANCE.Length))
                        eStateResponse = eState.Maintenance;
                    else if (data == BitConverter.ToString(Constants.OFFLINE, 0, Constants.OFFLINE.Length))
                        eStateResponse = eState.Offline;
                    else if (data == BitConverter.ToString(Constants.ONLINE, 0, Constants.ONLINE.Length))
                        eStateResponse = eState.Online;
                    else if (data == BitConverter.ToString(Constants.OUTOFSERVICE, 0, Constants.OUTOFSERVICE.Length))
                        eStateResponse = eState.OutofService;
                    else
                        eStateResponse = eState.Invalid;




                }
               
                return true;
            }
            else
            {
                return false;
            }
        }

        //public static byte[] OUTOFSERVICE = { 0x7F, 0x1 };
        //public static byte[] INSERVICE = { 0x7F, 0x2 };
        //public static byte[] OFFLINE = { 0x7F, 0x3 };

        //public static byte[] ONLINE = { 0x7F, 0x04 };
        //public static byte[] MAINTENANCE = { 0x7F, 0x05 };


        //public static byte[] RECEIVE = { 0x7F, 0xa };



        public bool SendCommand(string ip,byte[] status)
        {
            NetworkStream ns = null;
            TcpClient tcp = null;

            try
            {
                tcp = new TcpClient();

                tcp.SendTimeout = 500;
                tcp.ReceiveTimeout = 500;
                tcp.Connect(IPAddress.Parse(ip), 8008);
                ns = tcp.GetStream();
                ns.Write(status,0, status.Length);

                Byte[] response = new Byte[256];
                int byteCount = ns.Read(response, 0,256);


                string responseData = System.Text.Encoding.ASCII.GetString(response,0,byteCount);

                Byte[] result = TrimByteArray(response,byteCount);
                Debug.WriteLine("Terminal SendControl:" + ip);
                Debug.WriteLine("Send:" + BitConverter.ToString(status));
                Debug.WriteLine("Receive:" + BitConverter.ToString(result));

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (ns != null)
                    ns.Close();
                tcp.Close();
            }

        }

        #region Utility
        public static bool PingAddress(string host)
        {
            if (host == string.Empty) return false;

            Ping x = new Ping();
            PingReply pingReply = x.Send(IPAddress.Parse(host));
            if (pingReply.Status == IPStatus.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static byte[] TrimByteArray(byte[] received, int receiveLength)
        {
            byte[] result = new byte[receiveLength];

            Array.Copy(received, result, receiveLength);

            return result;
        }


        #endregion
    }
}
