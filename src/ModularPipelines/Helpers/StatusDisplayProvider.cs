using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Enums;

namespace ModularPipelines.Helpers;

/// <summary>
/// Provides display information for module status values.
/// Maps module status enums to consistent visual representations with icons and messages.
/// </summary>
/// <example>
/// <code>
/// // Get display info for a status
/// var info = StatusDisplayProvider.GetDisplayInfo(Status.Successful);
/// // Returns: StatusDisplayInfo with SuccessIcon and message template
///
/// // Format a complete status message
/// var message = StatusDisplayProvider.FormatStatusMessage("MyModule", Status.Failed);
/// // Result: "[red]✗[/] Module [cyan]MyModule[/] failed"
///
/// // Use in logging
/// var moduleName = module.GetType().Name;
/// var message = StatusDisplayProvider.FormatStatusMessage(moduleName, module.Status);
/// logger.Log(logLevel, message);
/// </code>
/// </example>
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
