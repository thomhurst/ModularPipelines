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
    public virtual bool? Force { get; set; }

    [BooleanCommandSwitch("-lock")]
    public virtual bool? Lock { get; set; }

    [CommandSwitch("-lock-timeout")]
    public virtual string? LockTimeout { get; set; }
}