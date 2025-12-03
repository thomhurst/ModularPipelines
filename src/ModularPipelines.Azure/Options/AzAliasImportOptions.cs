using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alias", "import")]
public record AzAliasImportOptions(
[property: CliOption("--source")] string Source
) : AzOptions;