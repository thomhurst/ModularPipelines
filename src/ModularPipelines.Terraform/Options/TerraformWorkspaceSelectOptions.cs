using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("workspace select")]
[ExcludeFromCodeCoverage]
public record TerraformWorkspaceSelectOptions([property: PositionalArgument(Position = Position.AfterSwitches)]
    string Name) : TerraformOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Directory { get; set; }

    [BooleanCommandSwitch("-or-create")]
    public virtual bool? OrCreate { get; set; }
}