using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("get")]
[ExcludeFromCodeCoverage]
public record TerraformGetOptions([property: PositionalArgument(Position = Position.AfterSwitches)]
    string Path) : TerraformOptions
{
    [BooleanCommandSwitch("-update")]
    public bool? Update { get; set; }

    [BooleanCommandSwitch("-no-color")]
    public bool? NoColor { get; set; }
}
