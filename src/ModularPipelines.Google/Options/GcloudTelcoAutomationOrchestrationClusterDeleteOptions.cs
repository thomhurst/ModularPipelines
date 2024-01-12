using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("telco-automation", "orchestration-cluster", "delete")]
public record GcloudTelcoAutomationOrchestrationClusterDeleteOptions(
[property: PositionalArgument] string OrchestrationCluster,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}