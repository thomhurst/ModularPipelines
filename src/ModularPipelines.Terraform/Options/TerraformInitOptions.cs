using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("init")]
[ExcludeFromCodeCoverage]
public record TerraformInitOptions : TerraformOptions
{
    [BooleanCommandSwitch("-input")]
    public virtual bool? Input { get; set; }

    [BooleanCommandSwitch("-lock")]
    public virtual bool? Lock { get; set; }

    [CommandSwitch("-lock-timeout")]
    public virtual string? LockTimeout { get; set; }

    [BooleanCommandSwitch("-no-color")]
    public virtual bool? NoColor { get; set; }

    [BooleanCommandSwitch("-upgrade")]
    public virtual bool? Upgrade { get; set; }

    [CommandSwitch("-from-module")]
    public virtual string? FromModule { get; set; }

    [BooleanCommandSwitch("-reconfigure")]
    public virtual bool? Reconfigure { get; set; }

    [BooleanCommandSwitch("-migrate-state")]
    public virtual bool? MigrateState { get; set; }

    [BooleanCommandSwitch("-force-copy")]
    public virtual bool? ForceCopy { get; set; }

    [BooleanCommandSwitch("-backend")]
    public virtual bool? Backend { get; set; }

    [CommandSwitch("-backend-config")]
    public virtual string? BackendConfig { get; set; }

    [BooleanCommandSwitch("-get")]
    public virtual bool? Get { get; set; }

    [BooleanCommandSwitch("-get-plugins")]
    public virtual bool? GetPlugins { get; set; }

    [CommandSwitch("-plugin-dir")]
    public virtual string? PluginDir { get; set; }

    [CommandSwitch("-lockfile")]
    public virtual string? Lockfile { get; set; }

    [BooleanCommandSwitch("-chdir")]
    public virtual bool? Chdir { get; set; }
}