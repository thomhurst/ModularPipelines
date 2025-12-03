using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aosm", "nfd", "generate-config")]
public record AzAosmNfdGenerateConfigOptions(
[property: CliOption("--definition-type")] string DefinitionType
) : AzOptions
{
    [CliOption("--output-file")]
    public string? OutputFile { get; set; }
}