using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("bicep", "generate-params")]
public record AzBicepGenerateParamsOptions(
[property: CliOption("--file")] string File
) : AzOptions
{
    [CliOption("--include-params")]
    public string? IncludeParams { get; set; }

    [CliFlag("--no-restore")]
    public bool? NoRestore { get; set; }

    [CliOption("--outdir")]
    public string? Outdir { get; set; }

    [CliOption("--outfile")]
    public string? Outfile { get; set; }

    [CliOption("--output-format")]
    public string? OutputFormat { get; set; }

    [CliOption("--stdout")]
    public string? Stdout { get; set; }
}