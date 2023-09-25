using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("apply")]
[ExcludeFromCodeCoverage]
public record TerraformApplyOptions : TerraformOptions
{
    [BooleanCommandSwitch("-auto-approve")]
    public bool? AutoApprove { get; set; }

    [BooleanCommandSwitch("-destroy")]
    public bool? Destroy { get; set; }

    [BooleanCommandSwitch("-refresh-only")]
    public bool? RefreshOnly { get; set; }

    [BooleanCommandSwitch("-compact-warnings")]
    public bool? CompactWarnings { get; set; }

    [BooleanCommandSwitch("-input")]
    public bool? Input { get; set; }

    [BooleanCommandSwitch("-json")]
    public bool? Json { get; set; }

    [BooleanCommandSwitch("-lock")]
    public bool? Lock { get; set; }

    [CommandSwitch("-lock-timeout")]
    public string? LockTimeout { get; set; }

    [BooleanCommandSwitch("-no-color")]
    public bool? NoColor { get; set; }

    [CommandSwitch("-parallelism")]
    public int? Parallelism { get; set; }

    [BooleanCommandSwitch("-state")]
    public bool? State { get; set; }

    [BooleanCommandSwitch("-state-out")]
    public bool? StateOut { get; set; }

    [BooleanCommandSwitch("-backup")]
    public bool? Backup { get; set; }

    [BooleanCommandSwitch("-chdir")]
    public bool? Chdir { get; set; }
}
