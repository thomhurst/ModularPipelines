using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Enums;

namespace ModularPipelines.Helpers;

[ExcludeFromCodeCoverage]
internal static class StatusFormatter
{
    public static string ToDisplayString(this Status status)
    {
        return status switch
        {
            Status.NotYetStarted => "Not Yet Started",
            Status.Processing => "Processing...",
            Status.Successful => "[green]Successful[/]",
            Status.Failed => "[red]Failed[/]",
            Status.IgnoredFailure => "[orange3]Ignored Failure[/]",
            Status.PipelineTerminated => "[red]Pipeline Terminated[/]",
            Status.TimedOut => "[red]Timed Out[/]",
            Status.Skipped => "[yellow]Skipped[/]",
            Status.Unknown => "[yellow]Unknown[/]",
            Status.Retried => "[yellow]Retried[/]",
            Status.UsedHistory => "[green3]Used History[/]",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null),
        };
    }
}