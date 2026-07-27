using Spectre.Console;

namespace ModularPipelines.Console;

internal static class ProgressDisplayStyle
{
    internal const string PendingMarker = ".";
    internal const string RunningMarker = ">";
    internal const string SucceededMarker = "+";
    internal const string FailedMarker = "!";
    internal const string SkippedMarker = "-";
    internal const string CompletedSpinnerText = " ";

    internal static Spinner Spinner { get; } = Spinner.Known.Ascii;

    internal static string FormatTask(string name, ProgressTaskStatus status, bool isSubModule = false)
    {
        var (style, marker) = status switch
        {
            ProgressTaskStatus.Pending => ("dim", PendingMarker),
            ProgressTaskStatus.Running => ("cyan", RunningMarker),
            ProgressTaskStatus.Succeeded => ("green", SucceededMarker),
            ProgressTaskStatus.Failed => ("red", FailedMarker),
            ProgressTaskStatus.Skipped => ("yellow", SkippedMarker),
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null),
        };

        var indentation = isSubModule ? "  " : string.Empty;
        return $"[{style}]{indentation}{marker} {name}[/]";
    }
}

internal enum ProgressTaskStatus
{
    Pending,
    Running,
    Succeeded,
    Failed,
    Skipped,
}
