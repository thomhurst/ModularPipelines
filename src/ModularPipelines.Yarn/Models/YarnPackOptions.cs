using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pack")]
public record YarnPackOptions : YarnOptions
{
    [BooleanCommandSwitch("--install-if-needed")]
    public bool? InstallIfNeeded { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }

    [CommandSwitch("--out")]
    public string? Out { get; set; }
}