using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliCommand("state list")]
[ExcludeFromCodeCoverage]
public record TerraformStateListOptions : TerraformOptions
{
    [CliOption("-state")]
    public virtual string? State { get; set; }

    [CliOption("-id")]
    public virtual string? Id { get; set; }
}