using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("force-unlock")]
[ExcludeFromCodeCoverage]
public record TerraformForceUnlockOptions([property: PositionalArgument(Position = Position.AfterSwitches)]
    string Lockid) : TerraformOptions
{
    [BooleanCommandSwitch("-force")]
    public virtual bool? Force { get; set; }
}