using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("plan")]
[ExcludeFromCodeCoverage]
public record TerraformPlanOptions : TerraformOptions
{
    [CommandSwitch("-out")]
    public string? Out { get; set; }

    [BooleanCommandSwitch("-destroy")]
    public bool? Destroy { get; set; }

    [BooleanCommandSwitch("-refresh-only")]
    public bool? RefreshOnly { get; set; }

    [BooleanCommandSwitch("-refresh")]
    public bool? Refresh { get; set; }

    [CommandSwitch("-replace")]
    public string? Replace { get; set; }

    [CommandSwitch("-target")]
    public string? Target { get; set; }

    [CommandSwitch("-var")]
    public IEnumerable<KeyValue>? Vars { get; set; }

    [CommandSwitch("-var-file")]
    public string? VarFile { get; set; }

    [BooleanCommandSwitch("-compact-warnings")]
    public bool? CompactWarnings { get; set; }

    [BooleanCommandSwitch("-detailed-exitcode")]
    public bool? DetailedExitcode { get; set; }

    [CommandSwitch("-generate-config-out")]
    public string? GenerateConfigOut { get; set; }

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

    [BooleanCommandSwitch("-chdir")]
    public bool? Chdir { get; set; }
}