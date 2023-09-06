using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("state mv")]
[ExcludeFromCodeCoverage]
public record TerraformStateMvOptions([property: PositionalArgument(Position = Position.AfterArguments)]
    string Source, [property: PositionalArgument(Position = Position.AfterArguments)]
    string Destination) : TerraformOptions
{
    [BooleanCommandSwitch("-dry-run")] public bool? DryRun { get; set; }

    [BooleanCommandSwitch("-lock")] public bool? Lock { get; set; }

    [CommandSwitch("-lock-timeout")] public string? LockTimeout { get; set; }

    [BooleanCommandSwitch("-ignore-remote-version")]
    public bool? IgnoreRemoteVersion { get; set; }

    [BooleanCommandSwitch("-backup")] public bool? Backup { get; set; }

    [BooleanCommandSwitch("-backup-out")] public bool? BackupOut { get; set; }

    [BooleanCommandSwitch("-state")] public bool? State { get; set; }

    [BooleanCommandSwitch("-state-out")] public bool? StateOut { get; set; }

    [CommandSwitch("-resource")] public string? Resource { get; set; }
}
