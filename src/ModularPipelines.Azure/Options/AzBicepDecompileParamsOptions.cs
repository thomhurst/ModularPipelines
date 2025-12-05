using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("bicep", "decompile-params")]
public record AzBicepDecompileParamsOptions(
[property: CliOption("--file")] string File
) : AzOptions
{
    [CliOption("--bicep-file")]
    public string? BicepFile { get; set; }

    [CliOption("--outdir")]
    public string? Outdir { get; set; }

    [CliOption("--outfile")]
    public string? Outfile { get; set; }

    [CliOption("--stdout")]
    public string? Stdout { get; set; }
}