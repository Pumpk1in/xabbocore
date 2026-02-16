using Xabbo.Messages;
using Xabbo.Messages.Flash;

namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when kicking a member from a group.
/// <para/>
/// Supported clients: <see cref="ClientType.Modern"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.KickMember"/></item>
/// </list>
/// </summary>
/// <param name="GroupId">The ID of the group to kick the member from.</param>
/// <param name="UserId">The ID of the user to kick from the group.</param>
public sealed record KickGroupMemberMsg(Id GroupId, Id UserId) : IMessage<KickGroupMemberMsg>
{
    static Identifier IMessage<KickGroupMemberMsg>.Identifier => Out.KickMember;

    static KickGroupMemberMsg IParser<KickGroupMemberMsg>.Parse(in PacketReader p) => new(
        GroupId: p.ReadId(),
        UserId: p.ReadId()
    );

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteId(GroupId);
        p.WriteId(UserId);
        p.WriteBool(false);
    }
}
