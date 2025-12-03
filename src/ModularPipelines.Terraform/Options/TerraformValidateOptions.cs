using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliCommand("validate")]
[ExcludeFromCodeCoverage]
public record TerraformValidateOptions : TerraformOptions
{
    [CliFlag("-json")]
    public virtual bool? Json { get; set; }

    [CliFlag("-no-color")]
    public virtual bool? NoColor { get; set; }
}