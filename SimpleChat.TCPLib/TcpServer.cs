using System.Net.Sockets;

namespace SimpleChat.TCPLib;

public class TcpServer : TcpBase
{
    public TcpServer(string ip, int port) : base(ip, port)
    {
    }

    public void Start()
    {
        Socket.Bind(EndPoint);
        Socket.Listen();
    }

    public async Task<TcpBase> AcceptAsync()
    {
        var socket = await Socket.AcceptAsync();
        return new TcpBase(socket);
    }
}
