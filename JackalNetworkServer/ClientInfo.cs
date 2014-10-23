using System.IO;
using System.Net.Sockets;
using JackalNetwork;

namespace JackalNetworkServer
{
    public class ClientInfo
    {
        public TcpClient TcpClient;
        public string ClientId;
        public bool InGame;
        public StreamReader reader;
        public StreamWriter writer;

        readonly object ioLock=new object();
        public bool IsTerminated;

        public NetworkMessage Query(NetworkMessage request)
        {
            var message = JsonHelper.SerialiazeWithType(request);
            string answerMessage;
            lock (ioLock)
            {
                writer.WriteLine(message);
                if (request is GameReport) return null;

                answerMessage = reader.ReadLine();
            }
            NetworkMessage answer = JsonHelper.DeserialiazeWithType<NetworkMessage>(answerMessage);
            return answer;
        }
    }
}