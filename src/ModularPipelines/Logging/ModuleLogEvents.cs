using Microsoft.Extensions.Logging;

namespace ModularPipelines.Logging;

internal static class ModuleLogEvents
{
    public static EventId Status { get; } = new(int.MinValue, "ModularPipelines.ModuleStatus");

    public static EventId IgnoredDependencyFailure { get; } =
        new(int.MinValue + 1, "ModularPipelines.IgnoredDependencyFailure");

    /// <summary>
    /// A module status that the console output group header already shows. It is delivered to
    /// logging providers but not rendered to the console a second time.
    /// </summary>
    public static EventId StatusShownInGroupHeader { get; } =
        new(int.MinValue + 2, "ModularPipelines.ModuleStatus.ShownInGroupHeader");

    public static void LogStatus(
        this ILogger logger,
        LogLevel logLevel,
        string message,
        bool shownInGroupHeader = false) =>
        logger.Log(logLevel, shownInGroupHeader ? StatusShownInGroupHeader : Status, message);

    public static bool IsStatus(EventId eventId) =>
        (eventId.Id == Status.Id
         && string.Equals(eventId.Name, Status.Name, StringComparison.Ordinal))
        || IsStatusShownInGroupHeader(eventId);

    public static bool IsStatusShownInGroupHeader(EventId eventId) =>
        eventId.Id == StatusShownInGroupHeader.Id
        && string.Equals(eventId.Name, StatusShownInGroupHeader.Name, StringComparison.Ordinal);

    public static void LogIgnoredDependencyFailure(this ILogger logger, Exception exception) =>
        logger.LogError(
            IgnoredDependencyFailure,
            exception,
            "Ignoring Exception due to 'AlwaysRun' set");

    public static bool IsBuildIssueSuppressed(EventId eventId) =>
        IsStatus(eventId)
        || (eventId.Id == IgnoredDependencyFailure.Id
            && string.Equals(
                eventId.Name,
                IgnoredDependencyFailure.Name,
                StringComparison.Ordinal));
}
