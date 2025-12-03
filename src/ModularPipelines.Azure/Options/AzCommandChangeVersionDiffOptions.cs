using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("command-change", "version-diff")]
public record AzCommandChangeVersionDiffOptions(
[property: CliOption("--base-version")] string BaseVersion,
[property: CliOption("--diff-version")] string DiffVersion
) : AzOptions
{
    [CliFlag("--only-break")]
    public bool? OnlyBreak { get; set; }

    [CliOption("--output-type")]
    public string? OutputType { get; set; }

    [CliOption("--target-module")]
    public string? TargetModule { get; set; }

    [CliFlag("--use-cache")]
    public bool? UseCache { get; set; }

    [CliOption("--version-diff-file")]
    public string? VersionDiffFile { get; set; }
}