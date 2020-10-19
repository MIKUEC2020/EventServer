using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace EventServer.Data
{
    public class DataHolder
    {
        private static DataHolder _instance;
        public static DataHolder Instance => _instance ??= new DataHolder();
        public HashSet<Tuple<TcpClient, StreamWriter>> Clients = new HashSet<Tuple<TcpClient, StreamWriter>>();
    }
}