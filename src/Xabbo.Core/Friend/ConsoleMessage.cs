using Xabbo.Messages;

namespace Xabbo.Core;

public class ConsoleMessage : IParserComposer<ConsoleMessage>
{
    public Id ChatId { get; set; }
    public int MessageType { get; set; }
    public int HabbiconId { get; set; }
    public string Content { get; set; } = "";
    public int SecondsSinceSent { get; set; }
    public string? Time { get; set; }
    public string MessageId { get; set; } = "";
    public int ConfirmationId { get; set; }
    public Id SenderId { get; set; }
    public string? SenderName { get; set; }
    public string SenderFigure { get; set; } = "";

    static ConsoleMessage IParser<ConsoleMessage>.Parse(in PacketReader p) => p.Client switch
    {
        ClientType.Shockwave => new()
        {
            MessageId = p.ReadString(),
            SenderId = p.ReadId(),
            Time = p.ReadString(),
            Content = p.ReadString().Replace('\r', '\n'),
        },
        not ClientType.Shockwave => ParseModern(in p)
    };

    private static ConsoleMessage ParseModern(in PacketReader p)
    {
        var msg = new ConsoleMessage
        {
            ChatId = p.ReadId(),
            MessageType = p.ReadInt(),
        };
        // MessageType 0 = plain text message, otherwise a Habbicon (custom image) referenced by id.
        if (msg.MessageType == 0)
            msg.Content = p.ReadString();
        else
        {
            msg.HabbiconId = p.ReadInt();
            msg.Content = $":habbicon:{msg.HabbiconId}:";
        }
        msg.SecondsSinceSent = p.ReadInt();
        msg.MessageId = p.ReadString();
        msg.ConfirmationId = p.ReadInt();
        msg.SenderId = p.ReadId();
        msg.SenderName = p.ReadString();
        msg.SenderFigure = p.ReadString();
        return msg;
    }

    void IComposer.Compose(in PacketWriter p)
    {
        if (p.Client is ClientType.Shockwave)
        {
            p.WriteString(MessageId);
            p.WriteId(SenderId);
            p.WriteString(Time ?? "");
            p.WriteString(Content.Replace('\n', '\r'));
        }
        else
        {
            p.WriteId(ChatId);
            p.WriteInt(MessageType);
            if (MessageType == 0)
                p.WriteString(Content);
            else
                p.WriteInt(HabbiconId);
            p.WriteInt(SecondsSinceSent);
            p.WriteString(MessageId);
            p.WriteInt(ConfirmationId);
            p.WriteId(SenderId);
            p.WriteString(SenderName ?? "");
            p.WriteString(SenderFigure);
        }
    }
}
