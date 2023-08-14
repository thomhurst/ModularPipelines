using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("state rm")]
public record TerraformStateRmOptions([property: PositionalArgument(Position = Position.AfterArguments)]
    IEnumerable<string> Address) : TerraformOptions
{
    [BooleanCommandSwitch("-dry-run")] public bool? DryRun { get; set; }

    [BooleanCommandSwitch("-lock")] public bool? Lock { get; set; }

    [CommandSwitch("-lock-timeout")] public string? LockTimeout { get; set; }

    [BooleanCommandSwitch("-ignore-remote-version")]
    public bool? IgnoreRemoteVersion { get; set; }

    [BooleanCommandSwitch("-state")] public bool? State { get; set; }

    [BooleanCommandSwitch("-state-out")] public bool? StateOut { get; set; }

    [BooleanCommandSwitch("-backup")] public bool? Backup { get; set; }
}