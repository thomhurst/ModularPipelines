using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("npm", "audit")]
public record YarnNpmAuditOptions : YarnOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliOption("--environment")]
    public virtual string? Environment { get; set; }

    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliFlag("--no-deprecations")]
    public virtual bool? NoDeprecations { get; set; }

    [CliOption("--severity")]
    public virtual string? Severity { get; set; }

    [CliOption("--exclude")]
    public virtual string? Exclude { get; set; }

    [CliOption("--ignore")]
    public virtual string? Ignore { get; set; }
}