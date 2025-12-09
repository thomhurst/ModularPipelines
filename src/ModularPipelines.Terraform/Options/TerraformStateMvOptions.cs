using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliSubCommand("state mv")]
[ExcludeFromCodeCoverage]
public record TerraformStateMvOptions([property: CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    string Source, [property: CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    string Destination) : TerraformOptions
{
    [CliFlag("-dry-run")]
    public virtual bool? DryRun { get; set; }

    [CliFlag("-lock")]
    public virtual bool? Lock { get; set; }

    [CliOption("-lock-timeout")]
    public virtual string? LockTimeout { get; set; }

    [CliFlag("-ignore-remote-version")]
    public virtual bool? IgnoreRemoteVersion { get; set; }

    [CliFlag("-backup")]
    public virtual bool? Backup { get; set; }

    [CliFlag("-backup-out")]
    public virtual bool? BackupOut { get; set; }

    [CliFlag("-state")]
    public virtual bool? State { get; set; }

    [CliFlag("-state-out")]
    public virtual bool? StateOut { get; set; }

    [CliOption("-resource")]
    public virtual string? Resource { get; set; }
}