using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("telco-automation", "orchestration-cluster", "describe")]
public record GcloudTelcoAutomationOrchestrationClusterDescribeOptions(
[property: PositionalArgument] string OrchestrationCluster,
[property: PositionalArgument] string Location
) : GcloudOptions;