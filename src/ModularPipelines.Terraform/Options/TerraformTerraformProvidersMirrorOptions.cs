using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliCommand("terraform providers mirror")]
[ExcludeFromCodeCoverage]
public record TerraformTerraformProvidersMirrorOptions : TerraformOptions
{
    [CliOption("-platform")]
    public virtual string? Platform { get; set; }
}