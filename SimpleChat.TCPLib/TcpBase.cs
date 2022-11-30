using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleChat.TCPLib;

public class TcpBase
{
    protected readonly Socket Socket; // public Socket Socket { get; init; }
    protected readonly IPEndPoint? EndPoint;

    protected TcpBase(string ip, int port)
    {
        Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //TODO IPAddress.TryParse(ip)
        EndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

        //TODO Logging
    }

    public TcpBase(Socket socket)
    {
        Socket = socket;
        EndPoint = null;
    }

    public async Task SendAsync(string message)
    {
        var bufferSend = Encoding.UTF8.GetBytes(message);
        await Socket.SendAsync(new ReadOnlyMemory<byte>(bufferSend), SocketFlags.None, CancellationToken.None);
    }

    public async Task<string> ReceiveAsync()
    {
        var receive = new StringBuilder();
        var temp = new byte[256];
        var buffer = new Memory<byte>(temp);
        var bytes = 0;
        do
        {
            bytes = await Socket.ReceiveAsync(buffer, SocketFlags.None, CancellationToken.None);
            temp = buffer.ToArray();
            receive.Append(Encoding.UTF8.GetString(temp, 0, bytes));
        } while (Socket.Available > 0);

        return receive.ToString();
    }
}
