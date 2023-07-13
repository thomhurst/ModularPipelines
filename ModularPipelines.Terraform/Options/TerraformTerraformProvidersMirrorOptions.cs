using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("terraform providers mirror")]
public record TerraformTerraformProvidersMirrorOptions : TerraformOptions
{
    [CommandSwitch("-platform")] public string? Platform { get; set; }
}