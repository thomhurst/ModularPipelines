using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("npm", "audit")]
public record YarnNpmAuditOptions : YarnOptions
{
    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CommandSwitch("--environment")]
    public virtual string? Environment { get; set; }

    [BooleanCommandSwitch("--json")]
    public virtual bool? Json { get; set; }

    [BooleanCommandSwitch("--no-deprecations")]
    public virtual bool? NoDeprecations { get; set; }

    [CommandSwitch("--severity")]
    public virtual string? Severity { get; set; }

    [CommandSwitch("--exclude")]
    public virtual string? Exclude { get; set; }

    [CommandSwitch("--ignore")]
    public virtual string? Ignore { get; set; }
}