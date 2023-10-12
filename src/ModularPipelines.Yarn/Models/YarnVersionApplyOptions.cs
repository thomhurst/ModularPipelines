using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("version", "apply")]
public record YarnVersionApplyOptions : YarnOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("--prerelease")]
    public bool? Prerelease { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }
}