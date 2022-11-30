using SimpleChat.TCPLib;

var tcpServer = new TcpServer("127.0.0.1", 8005);
tcpServer.Start();

while (true)
{
    var client = await tcpServer.AcceptAsync();

    await Task.Run(async () =>
    {
        while (true)
        {
            var receive = await client.ReceiveAsync();
#if DEBUG
            Console.WriteLine(receive);
#endif
        }
    });
}


