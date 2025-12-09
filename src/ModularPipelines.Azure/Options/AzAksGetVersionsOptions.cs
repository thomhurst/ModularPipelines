using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks", "get-versions")]
public record AzAksGetVersionsOptions(
[property: CliOption("--location")] string Location
) : AzOptions;