using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("terraform providers lock")]
[ExcludeFromCodeCoverage]
public record TerraformTerraformProvidersLockOptions : TerraformOptions
{
    [CommandSwitch("-fs-mirror")]
    public virtual string? FsMirror { get; set; }

    [CommandSwitch("-net-mirror")]
    public virtual string? NetMirror { get; set; }

    [CommandSwitch("-platform")]
    public virtual string? Platform { get; set; }
}