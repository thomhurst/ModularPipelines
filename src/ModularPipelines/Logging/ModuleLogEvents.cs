using Microsoft.Extensions.Logging;

namespace ModularPipelines.Logging;

internal static class ModuleLogEvents
{
    public static EventId Status { get; } = new(int.MinValue, "ModularPipelines.ModuleStatus");

    public static bool IsStatus(EventId eventId) =>
        eventId.Id == Status.Id
        && string.Equals(eventId.Name, Status.Name, StringComparison.Ordinal);
}
