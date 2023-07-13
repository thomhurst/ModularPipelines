using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("terraform providers lock")]
public record TerraformTerraformProvidersLockOptions : TerraformOptions
{
    [CommandSwitch("-fs-mirror")] public string? FsMirror { get; set; }

    [CommandSwitch("-net-mirror")] public string? NetMirror { get; set; }

    [CommandSwitch("-platform")] public string? Platform { get; set; }
}