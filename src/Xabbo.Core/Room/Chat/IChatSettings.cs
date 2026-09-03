namespace Xabbo.Core;

/// <summary>
/// Defines chat related settings for a room.
/// </summary>
public interface IChatSettings
{
    /// <summary>
    /// The chat flood protection level for the room.
    /// </summary>
    ChatFloodProtection FloodProtection { get; }
}
