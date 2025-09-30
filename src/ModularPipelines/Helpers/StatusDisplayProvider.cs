using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Enums;

namespace ModularPipelines.Helpers;

/// <summary>
/// Provides display information for module status values.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class StatusDisplayProvider
{
    private static readonly Dictionary<Status, StatusDisplayInfo> StatusDisplayMap = new()
    {
        [Status.NotYetStarted] = new(MarkupFormatter.WarningIcon, "Module {0} never started"),
        [Status.Processing] = new(MarkupFormatter.FailureIcon, "Module {0} didn't finish executing"),
        [Status.Successful] = new(MarkupFormatter.SuccessIcon, "Module {0} completed successfully"),
        [Status.Failed] = new(MarkupFormatter.FailureIcon, "Module {0} failed"),
        [Status.TimedOut] = new(MarkupFormatter.TimeoutIcon, "Module {0} timed out"),
        [Status.Skipped] = new(MarkupFormatter.SkipIcon, "Module {0} skipped"),
        [Status.Unknown] = new(MarkupFormatter.QuestionIcon, "Unknown status for module {0}"),
        [Status.IgnoredFailure] = new("[orange3]⚠[/]", "Module {0} failed but the failure was ignored"),
        [Status.PipelineTerminated] = new(MarkupFormatter.StopIcon, "Module {0} terminated due to pipeline error"),
        [Status.UsedHistory] = new(MarkupFormatter.HistoryIcon, "Module {0} used historical data"),
        [Status.Retried] = new(MarkupFormatter.RetryIcon, "Module {0} retried"),
    };

    /// <summary>
    /// Gets display information for a given status.
    /// </summary>
    public static StatusDisplayInfo GetDisplayInfo(Status status)
    {
        if (StatusDisplayMap.TryGetValue(status, out var info))
        {
            return info;
        }

        return new StatusDisplayInfo(MarkupFormatter.QuestionIcon, $"Module {{0}} has unknown status: {status}");
    }

    /// <summary>
    /// Formats a status message for a given module and status.
    /// </summary>
    public static string FormatStatusMessage(string moduleName, Status status)
    {
        var displayInfo = GetDisplayInfo(status);
        var formattedModuleName = MarkupFormatter.FormatModuleName(moduleName);
        return $"{displayInfo.Icon} {string.Format(displayInfo.MessageTemplate, formattedModuleName)}";
    }
}

/// <summary>
/// Represents display information for a status.
/// </summary>
[ExcludeFromCodeCoverage]
internal record StatusDisplayInfo(string Icon, string MessageTemplate);