using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bicep", "format")]
public record AzBicepFormatOptions(
[property: CommandSwitch("--file")] string File
) : AzOptions
{
    [CommandSwitch("--indent-kind")]
    public string? IndentKind { get; set; }

    [CommandSwitch("--indent-size")]
    public int? IndentSize { get; set; }

    [BooleanCommandSwitch("--insert-final-newline")]
    public bool? InsertFinalNewline { get; set; }

    [CommandSwitch("--newline")]
    public string? Newline { get; set; }

    [CommandSwitch("--outdir")]
    public string? Outdir { get; set; }

    [CommandSwitch("--outfile")]
    public string? Outfile { get; set; }

    [CommandSwitch("--stdout")]
    public string? Stdout { get; set; }
}