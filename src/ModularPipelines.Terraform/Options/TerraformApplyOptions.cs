using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("apply")]
[ExcludeFromCodeCoverage]
public record TerraformApplyOptions : TerraformOptions
{
    [BooleanCommandSwitch("-auto-approve")]
    public virtual bool? AutoApprove { get; set; }

    [BooleanCommandSwitch("-destroy")]
    public virtual bool? Destroy { get; set; }

    [BooleanCommandSwitch("-refresh-only")]
    public virtual bool? RefreshOnly { get; set; }

    [BooleanCommandSwitch("-compact-warnings")]
    public virtual bool? CompactWarnings { get; set; }

    [BooleanCommandSwitch("-input")]
    public virtual bool? Input { get; set; }

    [BooleanCommandSwitch("-json")]
    public virtual bool? Json { get; set; }

    [BooleanCommandSwitch("-lock")]
    public virtual bool? Lock { get; set; }

    [CommandSwitch("-lock-timeout")]
    public virtual string? LockTimeout { get; set; }

    [BooleanCommandSwitch("-no-color")]
    public virtual bool? NoColor { get; set; }

    [CommandSwitch("-parallelism")]
    public virtual int? Parallelism { get; set; }

    [BooleanCommandSwitch("-state")]
    public virtual bool? State { get; set; }

    [BooleanCommandSwitch("-state-out")]
    public virtual bool? StateOut { get; set; }

    [BooleanCommandSwitch("-backup")]
    public virtual bool? Backup { get; set; }

    [BooleanCommandSwitch("-chdir")]
    public virtual bool? Chdir { get; set; }
}