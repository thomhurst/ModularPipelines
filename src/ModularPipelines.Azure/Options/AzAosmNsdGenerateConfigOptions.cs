using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aosm", "nsd", "generate-config")]
public record AzAosmNsdGenerateConfigOptions : AzOptions
{
    [CliOption("--output-file")]
    public string? OutputFile { get; set; }
}