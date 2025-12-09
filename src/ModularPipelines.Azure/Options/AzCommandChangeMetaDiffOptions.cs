using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("command-change", "meta-diff")]
public record AzCommandChangeMetaDiffOptions(
[property: CliOption("--base-meta-file")] string BaseMetaFile,
[property: CliOption("--diff-meta-file")] string DiffMetaFile
) : AzOptions
{
    [CliFlag("--only-break")]
    public bool? OnlyBreak { get; set; }

    [CliOption("--output-file")]
    public string? OutputFile { get; set; }

    [CliOption("--output-type")]
    public string? OutputType { get; set; }
}