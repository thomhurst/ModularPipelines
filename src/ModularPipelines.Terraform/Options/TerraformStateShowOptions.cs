using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliSubCommand("state show")]
[ExcludeFromCodeCoverage]
public record TerraformStateShowOptions([property: CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    string Address) : TerraformOptions
{
    [CliOption("-state")]
    public virtual string? State { get; set; }
}