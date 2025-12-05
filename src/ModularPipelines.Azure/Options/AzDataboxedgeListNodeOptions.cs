using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("databoxedge", "list-node")]
public record AzDataboxedgeListNodeOptions(
[property: CliOption("--device-name")] string DeviceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;