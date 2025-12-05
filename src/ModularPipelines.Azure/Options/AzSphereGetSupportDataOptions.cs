using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "get-support-data")]
public record AzSphereGetSupportDataOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--output-file")] string OutputFile,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;