using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("command-change", "meta-diff")]
public record AzCommandChangeMetaDiffOptions(
[property: CommandSwitch("--base-meta-file")] string BaseMetaFile,
[property: CommandSwitch("--diff-meta-file")] string DiffMetaFile
) : AzOptions
{
    [BooleanCommandSwitch("--only-break")]
    public bool? OnlyBreak { get; set; }

    [CommandSwitch("--output-file")]
    public string? OutputFile { get; set; }

    [CommandSwitch("--output-type")]
    public string? OutputType { get; set; }
}