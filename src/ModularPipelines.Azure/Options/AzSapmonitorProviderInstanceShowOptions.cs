using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sapmonitor", "provider-instance", "show")]
public record AzSapmonitorProviderInstanceShowOptions(
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--provider-instance-name")] string ProviderInstanceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;