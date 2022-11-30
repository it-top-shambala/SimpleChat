using System.Net;
using System.Net.Sockets;
using System.Text;

var ipAddress = IPAddress.Parse("127.0.0.1");
var port = 8005;
var endPoint = new IPEndPoint(ipAddress, port);

var listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
listenerSocket.Bind(endPoint);
listenerSocket.Listen();

var clientSocket = listenerSocket.Accept();

var messageSend = "Hello";
var bufferSend = Encoding.UTF8.GetBytes(messageSend);
clientSocket.Send(bufferSend);

var messageRecieve = new StringBuilder();
var bufferRecieve = new byte[256];
var bytes = 0;
do
{
    bytes = clientSocket.Receive(bufferRecieve);
    messageRecieve.Append(Encoding.UTF8.GetString(bufferRecieve, 0, bytes));
} while (clientSocket.Available > 0);
