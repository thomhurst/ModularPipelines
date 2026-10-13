using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ModularPipelines.Distributed.Redis.Coordination;

/// <summary>
/// Encodes queued assignments as <c>{class}|{order}|{assignment}</c>. All queue members share score zero,
/// so Redis sorts them by these bytes: each capability class is one contiguous range whose first member
/// is the class's best item, and <see cref="RedisCoordinationScripts.Claim"/> reads one member per class.
/// </summary>
/// <remarks>
/// <c>{class}</c> is 16 hexadecimal characters. <c>{order}</c> is 67 characters: the inverted priority (1),
/// the inverted critical path weight in milliseconds (15), the enqueue time in UTC ticks (19) and a unique
/// suffix (32), so higher priority, heavier and earlier work sorts first. The Lua scripts rely on these widths.
/// </remarks>
internal static class RedisQueueMember
{
    internal const int ClassIdLength = 16;

    internal const int OrderLength = 67;

    /// <summary>The zero-based offset of the assignment JSON within a member.</summary>
    internal const int AssignmentOffset = ClassIdLength + 1 + OrderLength + 1;

    private const long MaximumCriticalPathMilliseconds = 999_999_999_999_999;

    public static QueueClass GetClass(ModuleAssignment assignment) =>
        new(assignment.RequiredCapabilities, assignment.AlwaysRun);

    public static string GetClassId(string classJson) =>
        Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(classJson)))[..ClassIdLength];

    public static string Create(string classId, ModuleAssignment assignment, string assignmentJson) =>
        string.Create(
            CultureInfo.InvariantCulture,
            $"{classId}|{GetOrder(assignment)}{Guid.NewGuid():N}|{assignmentJson}");

    public static ModuleAssignment ParseAssignment(string member) =>
        JsonSerializer.Deserialize<ModuleAssignment>(member.AsSpan(AssignmentOffset))!;

    internal static string GetOrder(ModuleAssignment assignment)
    {
        var priority = 9 - Math.Clamp((int) assignment.Priority, 0, 9);
        var weight = (long) Math.Round(
            Math.Clamp(assignment.CriticalPathWeight.TotalMilliseconds, 0, MaximumCriticalPathMilliseconds),
            MidpointRounding.AwayFromZero);
        var enqueuedAt = Math.Max(0, assignment.EnqueuedAt.UtcTicks);
        return string.Create(
            CultureInfo.InvariantCulture,
            $"{priority:D1}{MaximumCriticalPathMilliseconds - weight:D15}{enqueuedAt:D19}");
    }

    /// <summary>The scheduling class shared by assignments with the same requirements.</summary>
    internal sealed record QueueClass(CapabilityRequirement RequiredCapabilities, bool AlwaysRun);
}
