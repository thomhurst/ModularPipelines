using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aro", "get-versions")]
public record AzAroGetVersionsOptions(
[property: CliOption("--location")] string Location
) : AzOptions;