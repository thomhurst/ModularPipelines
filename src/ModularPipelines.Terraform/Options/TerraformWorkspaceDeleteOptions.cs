using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("workspace delete")]
[ExcludeFromCodeCoverage]
public record TerraformWorkspaceDeleteOptions([property: PositionalArgument(Position = Position.AfterSwitches)]
    string Name) : TerraformOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Directory { get; set; }

    [BooleanCommandSwitch("-force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("-lock")]
    public bool? Lock { get; set; }

    [CommandSwitch("-lock-timeout")]
    public string? LockTimeout { get; set; }
}