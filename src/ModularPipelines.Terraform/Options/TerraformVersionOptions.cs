using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("version")]
public record TerraformVersionOptions : TerraformOptions
{
    [BooleanCommandSwitch("-json")] public bool? Json { get; set; }
}