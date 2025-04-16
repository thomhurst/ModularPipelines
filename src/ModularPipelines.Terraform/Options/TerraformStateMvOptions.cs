using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("state mv")]
[ExcludeFromCodeCoverage]
public record TerraformStateMvOptions([property: PositionalArgument(Position = Position.AfterSwitches)]
    string Source, [property: PositionalArgument(Position = Position.AfterSwitches)]
    string Destination) : TerraformOptions
{
    [BooleanCommandSwitch("-dry-run")]
    public virtual bool? DryRun { get; set; }

    [BooleanCommandSwitch("-lock")]
    public virtual bool? Lock { get; set; }

    [CommandSwitch("-lock-timeout")]
    public virtual string? LockTimeout { get; set; }

    [BooleanCommandSwitch("-ignore-remote-version")]
    public virtual bool? IgnoreRemoteVersion { get; set; }

    [BooleanCommandSwitch("-backup")]
    public virtual bool? Backup { get; set; }

    [BooleanCommandSwitch("-backup-out")]
    public virtual bool? BackupOut { get; set; }

    [BooleanCommandSwitch("-state")]
    public virtual bool? State { get; set; }

    [BooleanCommandSwitch("-state-out")]
    public virtual bool? StateOut { get; set; }

    [CommandSwitch("-resource")]
    public virtual string? Resource { get; set; }
}