using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks", "mesh", "get-revisions")]
public record AzAksMeshGetRevisionsOptions(
[property: CliOption("--location")] string Location
) : AzOptions;