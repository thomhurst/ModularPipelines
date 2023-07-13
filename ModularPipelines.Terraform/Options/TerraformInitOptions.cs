using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("init")]
public record TerraformInitOptions : TerraformOptions
{
    [BooleanCommandSwitch("-input")] public bool? Input { get; set; }

    [BooleanCommandSwitch("-lock")] public bool? Lock { get; set; }

    [CommandSwitch("-lock-timeout")] public string? LockTimeout { get; set; }

    [BooleanCommandSwitch("-no-color")] public bool? NoColor { get; set; }

    [BooleanCommandSwitch("-upgrade")] public bool? Upgrade { get; set; }

    [CommandSwitch("-from-module")] public string? FromModule { get; set; }

    [BooleanCommandSwitch("-reconfigure")] public bool? Reconfigure { get; set; }

    [BooleanCommandSwitch("-migrate-state")]
    public bool? MigrateState { get; set; }

    [BooleanCommandSwitch("-force-copy")] public bool? ForceCopy { get; set; }

    [BooleanCommandSwitch("-backend")] public bool? Backend { get; set; }

    [CommandSwitch("-backend-config")] public string? BackendConfig { get; set; }

    [BooleanCommandSwitch("-get")] public bool? Get { get; set; }

    [BooleanCommandSwitch("-get-plugins")] public bool? GetPlugins { get; set; }

    [CommandSwitch("-plugin-dir")] public string? PluginDir { get; set; }

    [CommandSwitch("-lockfile")] public string? Lockfile { get; set; }

    [BooleanCommandSwitch("-chdir")] public bool? Chdir { get; set; }
}