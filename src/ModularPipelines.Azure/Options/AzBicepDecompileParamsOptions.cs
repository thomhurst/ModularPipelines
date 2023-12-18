using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bicep", "decompile-params")]
public record AzBicepDecompileParamsOptions(
[property: CommandSwitch("--file")] string File
) : AzOptions
{
    [CommandSwitch("--bicep-file")]
    public string? BicepFile { get; set; }

    [CommandSwitch("--outdir")]
    public string? Outdir { get; set; }

    [CommandSwitch("--outfile")]
    public string? Outfile { get; set; }

    [CommandSwitch("--stdout")]
    public string? Stdout { get; set; }
}