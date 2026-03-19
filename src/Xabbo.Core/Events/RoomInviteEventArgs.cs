namespace Xabbo.Core.Events;

/// <summary>
/// Provides data for the <see cref="Game.FriendManager.RoomInviteReceived"/> event.
/// </summary>
/// <param name="Friend">The friend that sent the invitation.</param>
/// <param name="Message">The invitation message.</param>
public sealed record RoomInviteEventArgs(IFriend Friend, string Message) : FriendEventArgs(Friend);
