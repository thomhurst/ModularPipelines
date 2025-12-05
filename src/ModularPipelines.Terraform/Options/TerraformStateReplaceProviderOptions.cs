using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliCommand("state replace-provider")]
[ExcludeFromCodeCoverage]
public record TerraformStateReplaceProviderOptions(
    [property: CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    string Fromproviderfqn, [property: CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    string Toproviderfqn) : TerraformOptions
{
    [CliFlag("-auto-approve")]
    public virtual bool? AutoApprove { get; set; }

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