using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("info")]
public record YarnInfoOptions : YarnOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [CommandSwitch("--extra")]
    public string? Extra { get; set; }

    [BooleanCommandSwitch("--cache")]
    public bool? Cache { get; set; }

    [BooleanCommandSwitch("--dependents")]
    public bool? Dependents { get; set; }

    [BooleanCommandSwitch("--manifest")]
    public bool? Manifest { get; set; }

    [BooleanCommandSwitch("--name-only")]
    public bool? NameOnly { get; set; }

    [BooleanCommandSwitch("--virtuals")]
    public bool? Virtuals { get; set; }

    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }
}