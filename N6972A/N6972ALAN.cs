﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace N6972A
{

    public class N6972ALAN
    {
        public IPEndPoint InstrIp { get; private set; }
        public Socket InstrSocket { get; private set; }

        public N6972ALAN(string instrIPAddress, int instrPortNo)
        {        
            InstrIp = new IPEndPoint(IPAddress.Parse(instrIPAddress), instrPortNo);
            InstrSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                if (!InstrSocket.Connected)
                    InstrSocket.Connect(this.InstrIp);
            }
            catch (SocketException e)
            {
                Console.WriteLine($"Unable to connect to server. {e.Message}" );
                throw new ApplicationException("Instrument at " + this.InstrIp.Address + ":" + this.InstrIp.Port + " is not connected");
            }
        }

        public string WriteReadLine(string command)
        {
            InstrSocket.Send(Encoding.ASCII.GetBytes(command + "\n"));
            byte[] data = new byte[1024];
            int receivedDataLength = InstrSocket.Receive(data);
            return Encoding.ASCII.GetString(data, 0, receivedDataLength).Trim();
        }
    }

}
