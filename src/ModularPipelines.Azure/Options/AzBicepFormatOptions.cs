using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("bicep", "format")]
public record AzBicepFormatOptions(
[property: CliOption("--file")] string File
) : AzOptions
{
    [CliOption("--indent-kind")]
    public string? IndentKind { get; set; }

    [CliOption("--indent-size")]
    public int? IndentSize { get; set; }

    [CliFlag("--insert-final-newline")]
    public bool? InsertFinalNewline { get; set; }

    [CliOption("--newline")]
    public string? Newline { get; set; }

    [CliOption("--outdir")]
    public string? Outdir { get; set; }

    [CliOption("--outfile")]
    public string? Outfile { get; set; }

    [CliOption("--stdout")]
    public string? Stdout { get; set; }
}