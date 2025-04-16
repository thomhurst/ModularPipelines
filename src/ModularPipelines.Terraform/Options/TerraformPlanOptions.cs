using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("plan")]
[ExcludeFromCodeCoverage]
public record TerraformPlanOptions : TerraformOptions
{
    [CommandSwitch("-out")]
    public virtual string? Out { get; set; }

    [BooleanCommandSwitch("-destroy")]
    public virtual bool? Destroy { get; set; }

    [BooleanCommandSwitch("-refresh-only")]
    public virtual bool? RefreshOnly { get; set; }

    [BooleanCommandSwitch("-refresh")]
    public virtual bool? Refresh { get; set; }

    [CommandSwitch("-replace")]
    public virtual string? Replace { get; set; }

    [CommandSwitch("-target")]
    public virtual string? Target { get; set; }

    [CommandSwitch("-var")]
    public virtual IEnumerable<KeyValue>? Vars { get; set; }

    [CommandSwitch("-var-file")]
    public virtual string? VarFile { get; set; }

    [BooleanCommandSwitch("-compact-warnings")]
    public virtual bool? CompactWarnings { get; set; }

    [BooleanCommandSwitch("-detailed-exitcode")]
    public virtual bool? DetailedExitcode { get; set; }

    [CommandSwitch("-generate-config-out")]
    public virtual string? GenerateConfigOut { get; set; }

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

    [BooleanCommandSwitch("-chdir")]
    public virtual bool? Chdir { get; set; }
}