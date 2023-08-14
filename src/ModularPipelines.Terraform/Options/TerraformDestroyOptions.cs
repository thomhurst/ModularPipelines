using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("destroy")]
public record TerraformDestroyOptions : TerraformOptions
{
    [BooleanCommandSwitch("-destroy")] public bool? Destroy { get; set; }
}