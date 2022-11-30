namespace SimpleChat.TCPLib;

public class TcpClient : TcpBase
{
    public TcpClient(string ip, int port) : base(ip, port)
    {
    }

    public async Task ConntectToServerAsync()
    {
        await Socket.ConnectAsync(EndPoint);
    }
}
