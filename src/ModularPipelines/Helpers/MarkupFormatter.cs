using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Helpers;

/// <summary>
/// Provides consistent markup formatting for logging output with colors and icons.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class MarkupFormatter
{
    // Status Icons
    public const string SuccessIcon = "[green]✓[/]";
    public const string FailureIcon = "[red]✗[/]";
    public const string WarningIcon = "[yellow]⚠[/]";
    public const string SkipIcon = "[yellow]⊘[/]";
    public const string TimeoutIcon = "[red]⏱[/]";
    public const string StopIcon = "[red]⏹[/]";
    public const string RetryIcon = "[yellow]↻[/]";
    public const string HistoryIcon = "[green3]↻[/]";
    public const string QuestionIcon = "[red]?[/]";
    public const string PlayIcon = "[bold cyan]▶[/]";

    /// <summary>
    /// Gets the appropriate color for an exit code.
    /// </summary>
    public static string GetExitCodeColor(int? exitCode) => exitCode == 0 ? "green" : "red";

    /// <summary>
    /// Gets the appropriate color for a success/failure status.
    /// </summary>
    public static string GetStatusColor(bool isSuccess) => isSuccess ? "green" : "red";

    /// <summary>
    /// Gets the appropriate color for an HTTP status code.
    /// </summary>
    public static string GetHttpStatusColor(int? statusCode) => statusCode is >= 200 and < 300 ? "green" : "red";

    /// <summary>
    /// Formats a module name with consistent styling.
    /// </summary>
    public static string FormatModuleName(string moduleName) => $"[cyan]{moduleName}[/]";

    /// <summary>
    /// Formats a header with consistent styling.
    /// </summary>
    public static string FormatHeader(string headerText) => $"[bold]{headerText}:[/]";

    /// <summary>
    /// Formats a bold header with a specific color.
    /// </summary>
    public static string FormatColoredHeader(string headerText, string color) => $"[bold {color}]{headerText}:[/]";

    /// <summary>
    /// Formats a duration value.
    /// </summary>
    public static string FormatDuration(TimeSpan? duration) => $"[bold]Duration:[/] {duration?.ToDisplayString()}";

    /// <summary>
    /// Formats a dim/muted text value.
    /// </summary>
    public static string FormatDim(string text) => $"[dim]{text}[/]";

    /// <summary>
    /// Formats a bold text value.
    /// </summary>
    public static string FormatBold(string text) => $"[bold]{text}[/]";
}