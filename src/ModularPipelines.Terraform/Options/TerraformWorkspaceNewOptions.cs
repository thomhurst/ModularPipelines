using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("workspace new")]
[ExcludeFromCodeCoverage]
public record TerraformWorkspaceNewOptions([property: PositionalArgument(Position = Position.AfterSwitches)]
    string Name) : TerraformOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Directory { get; set; }

    [BooleanCommandSwitch("-state")]
    public virtual bool? State { get; set; }

    [BooleanCommandSwitch("-lock")]
    public virtual bool? Lock { get; set; }

    [CommandSwitch("-lock-timeout")]
    public virtual string? LockTimeout { get; set; }
}