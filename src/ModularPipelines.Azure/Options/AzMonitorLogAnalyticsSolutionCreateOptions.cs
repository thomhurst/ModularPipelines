using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "log-analytics", "solution", "create")]
public record AzMonitorLogAnalyticsSolutionCreateOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--solution-type")] string SolutionType,
[property: CliOption("--workspace")] string Workspace
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}