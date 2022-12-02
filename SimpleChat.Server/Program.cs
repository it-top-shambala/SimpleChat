using System.Text.Json;
using Logger.File;
using SimpleChat.ModelMessages;
using SimpleChat.TCPLib;

var logger = new LogToFile();

var users = new Dictionary<string, TcpBase>();

var tcpServer = new TcpServer("127.0.0.1", 8005);
tcpServer.Start();
logger.Info("Start SERVER");

while (true)
{
    var client = await tcpServer.AcceptAsync();

    await Task.Run(async () =>
    {
        var exit = false;
        while (!exit)
        {
            var receive = await client.ReceiveAsync();
            var msg = JsonSerializer.Deserialize<Message>(receive);
            switch (msg.Type)
            {
                case TypeMessage.Start:
                    users.Add(msg.UserName, client);
                    logger.Info($"Подключение клиента {msg.UserName}");
                    break;
                case TypeMessage.Message:
                    foreach (var (u, c) in users)
                    {
                        if (u == msg.UserName) continue;

                        await c.SendAsync(msg.Msg);
                        logger.Info($"{msg.UserName} -> {u}");
                    }
                    break;
                case TypeMessage.Stop:
                    users.Remove(msg.UserName);
                    exit = true;
                    break;
            }
        }
    });
}
