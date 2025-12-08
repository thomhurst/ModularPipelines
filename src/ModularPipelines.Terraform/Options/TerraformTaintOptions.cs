using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliSubCommand("taint")]
[ExcludeFromCodeCoverage]
public record TerraformTaintOptions : TerraformOptions
{
    [CliFlag("-replace")]
    public virtual bool? Replace { get; set; }

    [CliFlag("-allow-missing")]
    public virtual bool? AllowMissing { get; set; }

    [CliFlag("-lock")]
    public virtual bool? Lock { get; set; }

    [CliOption("-lock-timeout")]
    public virtual string? LockTimeout { get; set; }

    [CliFlag("-ignore-remote-version")]
    public virtual bool? IgnoreRemoteVersion { get; set; }

    [CliFlag("-state")]
    public virtual bool? State { get; set; }

    [CliFlag("-state-out")]
    public virtual bool? StateOut { get; set; }

    [CliFlag("-backup")]
    public virtual bool? Backup { get; set; }
}