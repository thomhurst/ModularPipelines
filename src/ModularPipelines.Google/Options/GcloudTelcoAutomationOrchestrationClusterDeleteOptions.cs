using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("telco-automation", "orchestration-cluster", "delete")]
public record GcloudTelcoAutomationOrchestrationClusterDeleteOptions(
[property: CliArgument] string OrchestrationCluster,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}