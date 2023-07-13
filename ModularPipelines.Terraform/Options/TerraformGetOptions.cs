using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("get")]
public record TerraformGetOptions([property: PositionalArgument(Position = Position.AfterArguments)]
    string Path) : TerraformOptions
{
    [BooleanCommandSwitch("-update")] public bool? Update { get; set; }

    [BooleanCommandSwitch("-no-color")] public bool? NoColor { get; set; }
}