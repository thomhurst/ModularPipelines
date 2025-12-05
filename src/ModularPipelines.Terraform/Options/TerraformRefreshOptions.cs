using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliCommand("refresh")]
[ExcludeFromCodeCoverage]
public record TerraformRefreshOptions : TerraformOptions
{
    [CliFlag("-auto-approve")]
    public virtual bool? AutoApprove { get; set; }

    [CliFlag("-refresh-only")]
    public virtual bool? RefreshOnly { get; set; }
}