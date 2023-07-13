using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("console")]
public record TerraformConsoleOptions : TerraformOptions
{
    [BooleanCommandSwitch("-state")] public bool? State { get; set; }
}