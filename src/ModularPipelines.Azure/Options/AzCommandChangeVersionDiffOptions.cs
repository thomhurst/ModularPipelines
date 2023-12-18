using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("command-change", "version-diff")]
public record AzCommandChangeVersionDiffOptions(
[property: CommandSwitch("--base-version")] string BaseVersion,
[property: CommandSwitch("--diff-version")] string DiffVersion
) : AzOptions
{
    [BooleanCommandSwitch("--only-break")]
    public bool? OnlyBreak { get; set; }

    [CommandSwitch("--output-type")]
    public string? OutputType { get; set; }

    [CommandSwitch("--target-module")]
    public string? TargetModule { get; set; }

    [BooleanCommandSwitch("--use-cache")]
    public bool? UseCache { get; set; }

    [CommandSwitch("--version-diff-file")]
    public string? VersionDiffFile { get; set; }
}