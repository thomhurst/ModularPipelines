using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("State Command")]
public record TerraformStateCommandOptions : TerraformOptions
{
    [BooleanCommandSwitch("-backup")] public bool? Backup { get; set; }
}