using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Enums;

namespace ModularPipelines.Helpers;

[ExcludeFromCodeCoverage]
internal static class StatusFormatter
{
    public static string ToDisplayString(this ModuleStatus status)
    {
        return status switch
        {
            ModuleStatus.NotStarted => "Not Yet Started",
            ModuleStatus.Running => "Processing...",
            ModuleStatus.Succeeded => "[green]Successful[/]",
            ModuleStatus.Failed => "[red]Failed[/]",
            ModuleStatus.FailureIgnored => "[orange3]Ignored Failure[/]",
            ModuleStatus.Cancelled => "[red]Pipeline Terminated[/]",
            ModuleStatus.DependencyFailed => "[red]Dependency Failed[/]",
            ModuleStatus.TimedOut => "[red]Timed Out[/]",
            ModuleStatus.Skipped => "[yellow]Skipped[/]",
            ModuleStatus.Unknown => "[yellow]Unknown[/]",
            ModuleStatus.RestoredFromHistory => "[green3]Used History[/]",
            ModuleStatus.RestoredFromCache => "[green3]Cached Result[/]",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null),
        };
    }
}
