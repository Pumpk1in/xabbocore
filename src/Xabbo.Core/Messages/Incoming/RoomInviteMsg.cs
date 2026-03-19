using Xabbo.Messages;
using Xabbo.Messages.Flash;

namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received when a room invitation is received from a friend.
/// </summary>
public sealed record RoomInviteMsg(Id SenderId, string Message) : IMessage<RoomInviteMsg>
{
    static Identifier IMessage<RoomInviteMsg>.Identifier => In.RoomInvite;
    static RoomInviteMsg IParser<RoomInviteMsg>.Parse(in PacketReader p) => new(p.ReadId(), p.ReadString());
    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteId(SenderId);
        p.WriteString(Message);
    }
}
