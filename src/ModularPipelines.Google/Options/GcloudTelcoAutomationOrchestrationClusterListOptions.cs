using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("telco-automation", "orchestration-cluster", "list")]
public record GcloudTelcoAutomationOrchestrationClusterListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;