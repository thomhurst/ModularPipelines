using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliCommand("show")]
[ExcludeFromCodeCoverage]
public record TerraformShowOptions : TerraformOptions
{
    [CliFlag("-json")]
    public virtual bool? Json { get; set; }

    [CliFlag("-refresh")]
    public virtual bool? Refresh { get; set; }

    [CliFlag("-no-color")]
    public virtual bool? NoColor { get; set; }
}