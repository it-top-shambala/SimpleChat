namespace SimpleChat.ModelMessages;

public enum TypeMessage
{
    Message, Start, Stop
}

public class Message
{
    public TypeMessage Type { get; set; }
    public string? UserName { get; set; }
    public string? Msg { get; set; }
}
