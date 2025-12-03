using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliCommand("version")]
[ExcludeFromCodeCoverage]
public record TerraformVersionOptions : TerraformOptions
{
    [CliFlag("-json")]
    public virtual bool? Json { get; set; }
}