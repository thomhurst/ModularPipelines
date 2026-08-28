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
/// var info = StatusDisplayProvider.GetDisplayInfo(ModuleStatus.Succeeded);
/// // Returns: StatusDisplayInfo with SuccessIcon and message template
///
/// // Format a complete status message
/// var message = StatusDisplayProvider.FormatStatusMessage("MyModule", ModuleStatus.Failed);
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
    private static readonly Dictionary<ModuleStatus, StatusDisplayInfo> StatusDisplayMap = new()
    {
        [ModuleStatus.NotStarted] = new(MarkupFormatter.WarningIcon, "Module {0} never started"),
        [ModuleStatus.Running] = new(MarkupFormatter.FailureIcon, "Module {0} didn't finish executing"),
        [ModuleStatus.Succeeded] = new(MarkupFormatter.SuccessIcon, "Module {0} completed successfully"),
        [ModuleStatus.Failed] = new(MarkupFormatter.FailureIcon, "Module {0} failed"),
        [ModuleStatus.TimedOut] = new(MarkupFormatter.TimeoutIcon, "Module {0} timed out"),
        [ModuleStatus.Skipped] = new(MarkupFormatter.SkipIcon, "Module {0} skipped"),
        [ModuleStatus.Unknown] = new(MarkupFormatter.QuestionIcon, "Unknown status for module {0}"),
        [ModuleStatus.FailureIgnored] = new("[orange3]⚠[/]", "Module {0} failed but the failure was ignored"),
        [ModuleStatus.Cancelled] = new(MarkupFormatter.StopIcon, "Module {0} was cancelled"),
        [ModuleStatus.DependencyFailed] = new(MarkupFormatter.FailureIcon, "Module {0} did not run because a dependency failed"),
        [ModuleStatus.RestoredFromHistory] = new(MarkupFormatter.HistoryIcon, "Module {0} used historical data"),
        [ModuleStatus.RestoredFromCache] = new(MarkupFormatter.HistoryIcon, "Module {0} used a cached result"),
    };

    /// <summary>
    /// Gets display information for a given status.
    /// </summary>
    public static StatusDisplayInfo GetDisplayInfo(ModuleStatus status)
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
    public static string FormatStatusMessage(string moduleName, ModuleStatus status)
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
