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
    // TODO: identify field added in WIN63-202605181326 (always 0 in captures)
    public int UnknownInt1 { get; set; }
    public bool CanMute { get; set; }
    // TODO: identify field added in WIN63-202605181326
    public bool UnknownBool1 { get; set; }
    public ChatSettings ChatSettings { get; set; } = new();
    IChatSettings IRoomData.ChatSettings => ChatSettings;

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

        if (p.Client is ClientType.Flash)
            UnknownInt1 = p.ReadInt();

        CanMute = p.ReadBool();

        if (p.Client is ClientType.Flash)
        {
            UnknownBool1 = p.ReadBool();
            // ChatSettings moved to a separate In.RoomChatSettings packet in WIN63-202605181326.
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

        if (p.Client is ClientType.Flash)
            p.WriteInt(UnknownInt1);

        p.WriteBool(CanMute);

        if (p.Client is ClientType.Flash)
            p.WriteBool(UnknownBool1);
        else
            p.Compose(ChatSettings);
    }

    static RoomData IParser<RoomData>.Parse(in PacketReader p)
    {
        bool isEntering = false;
        if (p.Client != ClientType.Shockwave)
            isEntering = p.ReadBool();
        return new(isEntering, in p);
    }
}
