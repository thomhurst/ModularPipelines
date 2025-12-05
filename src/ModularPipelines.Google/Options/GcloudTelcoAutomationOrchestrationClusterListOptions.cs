using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("telco-automation", "orchestration-cluster", "list")]
public record GcloudTelcoAutomationOrchestrationClusterListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;