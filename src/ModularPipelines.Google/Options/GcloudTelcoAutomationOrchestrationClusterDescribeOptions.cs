using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("telco-automation", "orchestration-cluster", "describe")]
public record GcloudTelcoAutomationOrchestrationClusterDescribeOptions(
[property: CliArgument] string OrchestrationCluster,
[property: CliArgument] string Location
) : GcloudOptions;