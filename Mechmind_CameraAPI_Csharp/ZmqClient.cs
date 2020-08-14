﻿using NetMQ;
using NetMQ.Sockets;
using System;
using Google.Protobuf;
using System.Net.Sockets;
using System.Drawing;
using Mmind;
using System.IO;

namespace Mechmind_CameraAPI_Csharp
{
    class ZmqClient
    {
      
           
        const int port = 5577;
        RequestSocket client;
        private string addr = "";
        private byte[] reqbuf;
        private byte[] resbuf;
        public ZmqClient()
        { 
            client = new RequestSocket();
        }
        public int setAddr(string ip)
        {
            if (ip.Length == 0)
                return -1;
            if (addr.Length != 0)
                client.Disconnect(addr);
            addr = "tcp://" + ip + ":" + port.ToString();
            Console.WriteLine("Connect to " + addr);
            client.Connect(addr);
            return 0;
        }
        public void disconnect()
        {
            client.Disconnect(addr);
            addr = "";
        }
        public bool empty()
        {
            return addr.Length == 0;
        }
        public Mmind.Response sendReq(Mmind.Request req)
        {        
            reqbuf = new byte[req.CalculateSize()];
            Serialize(reqbuf, req);
            client.SendFrame(reqbuf);
            resbuf = client.ReceiveFrameBytes();
            Mmind.Response rel = Mmind.Response.Parser.ParseFrom(resbuf);
            return rel;
        }
        private void Serialize(byte[] data, Mmind.Request person)
        {
            using (CodedOutputStream cos = new CodedOutputStream(data))
            {
                person.WriteTo(cos);
            }
                
        }

        
        
    }
}
