using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleChat.TCPLib;

public abstract class TcpBase
{
    private readonly Socket _socket; // public Socket Socket { get; init; }
    private readonly IPEndPoint _endPoint;

    protected TcpBase(string ip, int port)
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        //TODO IPAddress.TryParse(ip)
        _endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        
        //TODO Logging
    }

    public async Task SendAsync(string message)
    {
        var bufferSend = Encoding.UTF8.GetBytes(message);
        await _socket.SendAsync(new ReadOnlyMemory<byte>(bufferSend), SocketFlags.None, CancellationToken.None);
    }

    public async Task<string> Receive()
    {
        var receive = new StringBuilder();
        var temp = new byte[256];
        var buffer = new Memory<byte>(temp);
        var bytes = 0;
        do
        {
            bytes = await _socket.ReceiveAsync(buffer, SocketFlags.None, CancellationToken.None);
            temp = buffer.ToArray();
            receive.Append(Encoding.UTF8.GetString(temp, 0, bytes));
        } while (_socket.Available > 0);

        return receive.ToString();
    }
}