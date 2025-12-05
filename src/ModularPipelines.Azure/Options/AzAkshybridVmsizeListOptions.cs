using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("akshybrid", "vmsize", "list")]
public record AzAkshybridVmsizeListOptions(
[property: CliOption("--custom-location")] string CustomLocation,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;