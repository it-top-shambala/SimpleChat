using System.Windows.Input;
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

    public LambdaCommand SendMessageCommand { get; set; }

    private TcpClient _tcp;

    public MainWindowViewModel()
    {
        _message = string.Empty;
        _tcp = new TcpClient("127.0.0.1", 8005);

        SendMessageCommand = new LambdaCommand(
            _ => !string.IsNullOrWhiteSpace(Message),
            async _ =>
            {
                await _tcp.SendAsync(Message);
            });
    }
}
