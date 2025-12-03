using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliCommand("workspace new")]
[ExcludeFromCodeCoverage]
public record TerraformWorkspaceNewOptions([property: CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    string Name) : TerraformOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Directory { get; set; }

    [CliFlag("-state")]
    public virtual bool? State { get; set; }

    [CliFlag("-lock")]
    public virtual bool? Lock { get; set; }

    [CliOption("-lock-timeout")]
    public virtual string? LockTimeout { get; set; }
}