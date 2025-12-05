using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliCommand("State Command")]
[ExcludeFromCodeCoverage]
public record TerraformStateCommandOptions : TerraformOptions
{
    [CliFlag("-backup")]
    public virtual bool? Backup { get; set; }
}