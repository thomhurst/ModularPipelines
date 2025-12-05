using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliCommand("force-unlock")]
[ExcludeFromCodeCoverage]
public record TerraformForceUnlockOptions([property: CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    string Lockid) : TerraformOptions
{
    [CliFlag("-force")]
    public virtual bool? Force { get; set; }
}