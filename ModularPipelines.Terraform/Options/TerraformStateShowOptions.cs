using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("state show")]
public record TerraformStateShowOptions([property: PositionalArgument(Position = Position.AfterArguments)]
    string Address) : TerraformOptions
{
    [CommandSwitch("-state")] public string? State { get; set; }
}