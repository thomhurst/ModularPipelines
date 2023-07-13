using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("terraform providers schema")]
public record TerraformTerraformProvidersSchemaOptions : TerraformOptions
{
    [BooleanCommandSwitch("-json")] public bool? Json { get; set; }
}