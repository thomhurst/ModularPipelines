using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("import")]
[ExcludeFromCodeCoverage]
public record TerraformImportOptions([property: PositionalArgument(Position = Position.AfterSwitches)]
    string Address, [property: PositionalArgument(Position = Position.AfterSwitches)]
    string Id) : TerraformOptions
{
    [CommandSwitch("-config")]
    public virtual string? Config { get; set; }

    [BooleanCommandSwitch("-input")]
    public virtual bool? Input { get; set; }

    [BooleanCommandSwitch("-lock")]
    public virtual bool? Lock { get; set; }

    [CommandSwitch("-lock-timeout")]
    public virtual string? LockTimeout { get; set; }

    [BooleanCommandSwitch("-no-color")]
    public virtual bool? NoColor { get; set; }

    [CommandSwitch("-parallelism")]
    public virtual int? Parallelism { get; set; }

    [CommandSwitch("-provider")]
    public virtual string? Provider { get; set; }

    [CommandSwitch("-var")]
    public virtual IEnumerable<KeyValue>? Vars { get; set; }

    [CommandSwitch("-var-file")]
    public virtual string? VarFile { get; set; }

    [BooleanCommandSwitch("-ignore-remote-version")]
    public virtual bool? IgnoreRemoteVersion { get; set; }

    [BooleanCommandSwitch("-state")]
    public virtual bool? State { get; set; }

    [BooleanCommandSwitch("-state-out")]
    public virtual bool? StateOut { get; set; }

    [BooleanCommandSwitch("-backup")]
    public virtual bool? Backup { get; set; }
}