using Xabbo.Messages;

namespace Xabbo.Core;

/// <inheritdoc cref="IChatSettings"/>.
public class ChatSettings : IChatSettings, IParserComposer<ChatSettings>
{
    public ChatFloodProtection FloodProtection { get; set; }

    public ChatSettings()
    {
        FloodProtection = ChatFloodProtection.Standard;
    }

    internal ChatSettings(in PacketReader p)
    {
        FloodProtection = (ChatFloodProtection)p.ReadInt();
    }

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt((int)FloodProtection);
    }

    static ChatSettings IParser<ChatSettings>.Parse(in PacketReader p) => new(in p);
}
