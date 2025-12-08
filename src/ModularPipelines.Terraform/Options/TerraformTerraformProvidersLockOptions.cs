using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CliSubCommand("terraform providers lock")]
[ExcludeFromCodeCoverage]
public record TerraformTerraformProvidersLockOptions : TerraformOptions
{
    [CliOption("-fs-mirror")]
    public virtual string? FsMirror { get; set; }

    [CliOption("-net-mirror")]
    public virtual string? NetMirror { get; set; }

    [CliOption("-platform")]
    public virtual string? Platform { get; set; }
}