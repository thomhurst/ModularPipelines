using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("logz", "sub-account", "list-resource")]
public record AzLogzSubAccountListResourceOptions(
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;