using Xabbo.Messages;

namespace Xabbo.Core;

/// <inheritdoc cref="IRoomData"/>
public class RoomData : RoomInfo, IRoomData, IParserComposer<RoomData>
{
    public bool IsEntering { get; set; }
    public bool Forward { get; set; }
    public bool IsStaffPick { get; set; }
    public bool IsGroupMember { get; set; }
    public bool IsRoomMuted { get; set; }
    public ModerationSettings Moderation { get; set; } = new();
    IModerationSettings IRoomData.Moderation => Moderation;
    public bool CanMute { get; set; }
    public ChatSettings ChatSettings { get; set; } = new();
    IChatSettings IRoomData.ChatSettings => ChatSettings;
    /// <summary>
    /// Whether the connection to the room is being opened. (Flash only, WIN63-202605181326.)
    /// </summary>
    public bool OpeningConnection { get; set; }

    public RoomData()
    {
        Moderation = new ModerationSettings();
        ChatSettings = new ChatSettings();
    }

    protected RoomData(bool isEntering, in PacketReader p) : base(in p)
    {
        if (p.Client == ClientType.Shockwave)
            return;

        IsEntering = isEntering;

        Forward = p.ReadBool();
        IsStaffPick = p.ReadBool();
        IsGroupMember = p.ReadBool();
        IsRoomMuted = p.ReadBool();

        Moderation = p.Parse<ModerationSettings>();

        CanMute = p.ReadBool();

        if (p.Client is ClientType.Flash)
        {
            // WIN63-202605181326: ChatSettings reduced to a single FloodProtection int,
            // followed by an OpeningConnection bool.
            ChatSettings = new ChatSettings { FloodProtection = (ChatFloodProtection)p.ReadInt() };
            OpeningConnection = p.ReadBool();
        }
        else
        {
            ChatSettings = p.Parse<ChatSettings>();
        }
    }

    protected override void Compose(in PacketWriter p)
    {
        if (p.Client is not ClientType.Shockwave)
            p.WriteBool(IsEntering);

        base.Compose(in p);

        if (p.Client is ClientType.Shockwave)
            return;

        p.WriteBool(Forward);
        p.WriteBool(IsStaffPick);
        p.WriteBool(IsGroupMember);
        p.WriteBool(IsRoomMuted);

        p.Compose(Moderation);

        p.WriteBool(CanMute);

        if (p.Client is ClientType.Flash)
        {
            p.WriteInt((int)ChatSettings.FloodProtection);
            p.WriteBool(OpeningConnection);
        }
        else
        {
            p.Compose(ChatSettings);
        }
    }

    static RoomData IParser<RoomData>.Parse(in PacketReader p)
    {
        bool isEntering = false;
        if (p.Client != ClientType.Shockwave)
            isEntering = p.ReadBool();
        return new(isEntering, in p);
    }
}
