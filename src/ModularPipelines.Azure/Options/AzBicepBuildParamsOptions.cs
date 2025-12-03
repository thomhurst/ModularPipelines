using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bicep", "build-params")]
public record AzBicepBuildParamsOptions(
[property: CliOption("--file")] string File
) : AzOptions
{
    [CliFlag("--no-restore")]
    public bool? NoRestore { get; set; }

    [CliOption("--outdir")]
    public string? Outdir { get; set; }

    [CliOption("--outfile")]
    public string? Outfile { get; set; }

    [CliOption("--stdout")]
    public string? Stdout { get; set; }
}