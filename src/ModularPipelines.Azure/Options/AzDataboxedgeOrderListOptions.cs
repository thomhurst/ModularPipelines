using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("databoxedge", "order", "list")]
public record AzDataboxedgeOrderListOptions(
[property: CliOption("--device-name")] string DeviceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;