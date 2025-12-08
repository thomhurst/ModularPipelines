using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliSubCommand("console")]
[ExcludeFromCodeCoverage]
public record TerraformConsoleOptions : TerraformOptions
{
    [CliFlag("-state")]
    public virtual bool? State { get; set; }
}