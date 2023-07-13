using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("taint")]
public record TerraformTaintOptions : TerraformOptions
{
    [BooleanCommandSwitch("-replace")] public bool? Replace { get; set; }

    [BooleanCommandSwitch("-allow-missing")]
    public bool? AllowMissing { get; set; }

    [BooleanCommandSwitch("-lock")] public bool? Lock { get; set; }

    [CommandSwitch("-lock-timeout")] public string? LockTimeout { get; set; }

    [BooleanCommandSwitch("-ignore-remote-version")]
    public bool? IgnoreRemoteVersion { get; set; }

    [BooleanCommandSwitch("-state")] public bool? State { get; set; }

    [BooleanCommandSwitch("-state-out")] public bool? StateOut { get; set; }

    [BooleanCommandSwitch("-backup")] public bool? Backup { get; set; }
}