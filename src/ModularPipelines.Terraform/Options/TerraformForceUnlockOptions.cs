using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("force-unlock")]
public record TerraformForceUnlockOptions([property: PositionalArgument(Position = Position.AfterArguments)]
    string Lockid) : TerraformOptions
{
    [BooleanCommandSwitch("-force")] public bool? Force { get; set; }
}