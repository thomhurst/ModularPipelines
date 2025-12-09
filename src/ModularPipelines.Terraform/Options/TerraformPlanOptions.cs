using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Terraform.Options;

[CliSubCommand("plan")]
[ExcludeFromCodeCoverage]
public record TerraformPlanOptions : TerraformOptions
{
    [CliOption("-out")]
    public virtual string? Out { get; set; }

    [CliFlag("-destroy")]
    public virtual bool? Destroy { get; set; }

    [CliFlag("-refresh-only")]
    public virtual bool? RefreshOnly { get; set; }

    [CliFlag("-refresh")]
    public virtual bool? Refresh { get; set; }

    [CliOption("-replace")]
    public virtual string? Replace { get; set; }

    [CliOption("-target")]
    public virtual string? Target { get; set; }

    [CliOption("-var")]
    public virtual IEnumerable<KeyValue>? Vars { get; set; }

    [CliOption("-var-file")]
    public virtual string? VarFile { get; set; }

    [CliFlag("-compact-warnings")]
    public virtual bool? CompactWarnings { get; set; }

    [CliFlag("-detailed-exitcode")]
    public virtual bool? DetailedExitcode { get; set; }

    [CliOption("-generate-config-out")]
    public virtual string? GenerateConfigOut { get; set; }

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

    [CliFlag("-chdir")]
    public virtual bool? Chdir { get; set; }
}