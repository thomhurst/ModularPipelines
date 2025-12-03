using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "get-os-options")]
public record AzAksGetOsOptionsOptions(
[property: CliOption("--location")] string Location
) : AzOptions;