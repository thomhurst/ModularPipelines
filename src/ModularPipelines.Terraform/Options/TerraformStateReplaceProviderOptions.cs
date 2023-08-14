using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("state replace-provider")]
public record TerraformStateReplaceProviderOptions(
    [property: PositionalArgument(Position = Position.AfterArguments)]
    string Fromproviderfqn, [property: PositionalArgument(Position = Position.AfterArguments)]
    string Toproviderfqn) : TerraformOptions
{
    [BooleanCommandSwitch("-auto-approve")]
    public bool? AutoApprove { get; set; }

    [BooleanCommandSwitch("-lock")] public bool? Lock { get; set; }

    [CommandSwitch("-lock-timeout")] public string? LockTimeout { get; set; }

    [BooleanCommandSwitch("-ignore-remote-version")]
    public bool? IgnoreRemoteVersion { get; set; }

    [BooleanCommandSwitch("-state")] public bool? State { get; set; }

    [BooleanCommandSwitch("-state-out")] public bool? StateOut { get; set; }

    [BooleanCommandSwitch("-backup")] public bool? Backup { get; set; }
}