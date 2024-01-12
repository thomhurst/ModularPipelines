using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("assured", "workloads", "enable-resource-monitoring")]
public record GcloudAssuredWorkloadsEnableResourceMonitoringOptions(
[property: PositionalArgument] string Workload,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Organization
) : GcloudOptions;