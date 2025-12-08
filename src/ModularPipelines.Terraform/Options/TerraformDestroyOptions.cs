using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliSubCommand("destroy")]
[ExcludeFromCodeCoverage]
public record TerraformDestroyOptions : TerraformOptions
{
    [CliFlag("-destroy")]
    public virtual bool? Destroy { get; set; }
}