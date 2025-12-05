using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliCommand("workspace delete")]
[ExcludeFromCodeCoverage]
public record TerraformWorkspaceDeleteOptions([property: CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    string Name) : TerraformOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Directory { get; set; }

    [CliFlag("-force")]
    public virtual bool? Force { get; set; }

    [CliFlag("-lock")]
    public virtual bool? Lock { get; set; }

    [CliOption("-lock-timeout")]
    public virtual string? LockTimeout { get; set; }
}