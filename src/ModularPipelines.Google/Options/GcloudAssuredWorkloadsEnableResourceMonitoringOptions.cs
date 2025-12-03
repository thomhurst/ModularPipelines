using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("assured", "workloads", "enable-resource-monitoring")]
public record GcloudAssuredWorkloadsEnableResourceMonitoringOptions(
[property: CliArgument] string Workload,
[property: CliArgument] string Location,
[property: CliArgument] string Organization
) : GcloudOptions;