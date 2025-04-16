using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("get")]
[ExcludeFromCodeCoverage]
public record TerraformGetOptions([property: PositionalArgument(Position = Position.AfterSwitches)]
    string Path) : TerraformOptions
{
    [BooleanCommandSwitch("-update")]
    public virtual bool? Update { get; set; }

    [BooleanCommandSwitch("-no-color")]
    public virtual bool? NoColor { get; set; }
}