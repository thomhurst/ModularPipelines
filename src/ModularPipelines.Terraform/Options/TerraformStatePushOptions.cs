using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliCommand("state push")]
[ExcludeFromCodeCoverage]
public record TerraformStatePushOptions([property: CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    string Path) : TerraformOptions
{
    [CliFlag("-force")]
    public virtual bool? Force { get; set; }

    [CliFlag("-ignore-remote-version")]
    public virtual bool? IgnoreRemoteVersion { get; set; }
}