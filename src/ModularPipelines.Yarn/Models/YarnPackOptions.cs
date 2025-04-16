using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pack")]
public record YarnPackOptions : YarnOptions
{
    [BooleanCommandSwitch("--install-if-needed")]
    public virtual bool? InstallIfNeeded { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [BooleanCommandSwitch("--json")]
    public virtual bool? Json { get; set; }

    [CommandSwitch("--out")]
    public virtual string? Out { get; set; }
}