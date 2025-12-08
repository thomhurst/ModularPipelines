using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliSubCommand("terraform providers schema")]
[ExcludeFromCodeCoverage]
public record TerraformTerraformProvidersSchemaOptions : TerraformOptions
{
    [CliFlag("-json")]
    public virtual bool? Json { get; set; }
}