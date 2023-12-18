using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bicep", "generate-params")]
public record AzBicepGenerateParamsOptions(
[property: CommandSwitch("--file")] string File
) : AzOptions
{
    [CommandSwitch("--include-params")]
    public string? IncludeParams { get; set; }

    [BooleanCommandSwitch("--no-restore")]
    public bool? NoRestore { get; set; }

    [CommandSwitch("--outdir")]
    public string? Outdir { get; set; }

    [CommandSwitch("--outfile")]
    public string? Outfile { get; set; }

    [CommandSwitch("--output-format")]
    public string? OutputFormat { get; set; }

    [CommandSwitch("--stdout")]
    public string? Stdout { get; set; }
}