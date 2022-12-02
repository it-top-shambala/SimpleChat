using System;
using System.Text.Json;
using System.Threading.Tasks;
using SimpleChat.ModelMessages;
using SimpleChat.TCPLib;

namespace SimpleChat.Client.ViewModels;

public class MainWindowViewModel : BaseNotification
{
    private string _message;
    public string Message
    {
        get => _message;
        set
        {
            SetField(ref _message, value);
            SendMessageCommand.OnCanExecuteChanged();
        }
    }

    private string _output;
    public string Output
    {
        get => _output;
        set => SetField(ref _output, value);
    }

    public LambdaCommand SendMessageCommand { get; set; }

    private readonly TcpClient _tcp;

    private string _userName;

    public MainWindowViewModel()
    {
        _message = string.Empty;
        _output = string.Empty;

        _userName = Guid.NewGuid().ToString();

        _tcp = new TcpClient("127.0.0.1", 8005);
        _tcp.ConntectToServerAsync();
        _tcp.SendAsync(JsonSerializer.Serialize(new Message
        {
            Type = TypeMessage.Start,
            UserName = _userName
        }));

        ReceiveAsync();

        SendMessageCommand = new LambdaCommand(
            _ => !string.IsNullOrWhiteSpace(Message),
            async _ => { await SendAsync(); });
    }

    private async Task SendAsync()
    {
        var msg = new Message
        {
            Type = TypeMessage.Message,
            UserName = _userName,
            Msg = Message
        };
        var jsonMsg = JsonSerializer.Serialize(msg);
        await _tcp.SendAsync(jsonMsg);
    }

    private async Task ReceiveAsync()
    {
        await Task.Run(async () =>
        {
            while (true)
            {
                Output += await _tcp.ReceiveAsync();
            }
        });
    }

    /*private string GetUserName()
    {
        var guid = Guid.NewGuid();
        return guid.ToString();
    }*/
}
