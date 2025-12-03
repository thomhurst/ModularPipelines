using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliCommand("apply")]
[ExcludeFromCodeCoverage]
public record TerraformApplyOptions : TerraformOptions
{
    [CliFlag("-auto-approve")]
    public virtual bool? AutoApprove { get; set; }

    [CliFlag("-destroy")]
    public virtual bool? Destroy { get; set; }

    [CliFlag("-refresh-only")]
    public virtual bool? RefreshOnly { get; set; }

    [CliFlag("-compact-warnings")]
    public virtual bool? CompactWarnings { get; set; }

    [CliFlag("-input")]
    public virtual bool? Input { get; set; }

    [CliFlag("-json")]
    public virtual bool? Json { get; set; }

    [CliFlag("-lock")]
    public virtual bool? Lock { get; set; }

    [CliOption("-lock-timeout")]
    public virtual string? LockTimeout { get; set; }

    [CliFlag("-no-color")]
    public virtual bool? NoColor { get; set; }

    [CliOption("-parallelism")]
    public virtual int? Parallelism { get; set; }

    [CliFlag("-state")]
    public virtual bool? State { get; set; }

    [CliFlag("-state-out")]
    public virtual bool? StateOut { get; set; }

    [CliFlag("-backup")]
    public virtual bool? Backup { get; set; }

    [CliFlag("-chdir")]
    public virtual bool? Chdir { get; set; }
}