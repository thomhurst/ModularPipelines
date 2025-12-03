using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliCommand("init")]
[ExcludeFromCodeCoverage]
public record TerraformInitOptions : TerraformOptions
{
    [CliFlag("-input")]
    public virtual bool? Input { get; set; }

    [CliFlag("-lock")]
    public virtual bool? Lock { get; set; }

    [CliOption("-lock-timeout")]
    public virtual string? LockTimeout { get; set; }

    [CliFlag("-no-color")]
    public virtual bool? NoColor { get; set; }

    [CliFlag("-upgrade")]
    public virtual bool? Upgrade { get; set; }

    [CliOption("-from-module")]
    public virtual string? FromModule { get; set; }

    [CliFlag("-reconfigure")]
    public virtual bool? Reconfigure { get; set; }

    [CliFlag("-migrate-state")]
    public virtual bool? MigrateState { get; set; }

    [CliFlag("-force-copy")]
    public virtual bool? ForceCopy { get; set; }

    [CliFlag("-backend")]
    public virtual bool? Backend { get; set; }

    [CliOption("-backend-config")]
    public virtual string? BackendConfig { get; set; }

    [CliFlag("-get")]
    public virtual bool? Get { get; set; }

    [CliFlag("-get-plugins")]
    public virtual bool? GetPlugins { get; set; }

    [CliOption("-plugin-dir")]
    public virtual string? PluginDir { get; set; }

    [CliOption("-lockfile")]
    public virtual string? Lockfile { get; set; }

    [CliFlag("-chdir")]
    public virtual bool? Chdir { get; set; }
}