using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Terraform.Options;

[CommandPrecedingArguments("state push")]
[ExcludeFromCodeCoverage]
public record TerraformStatePushOptions([property: PositionalArgument(Position = Position.AfterSwitches)]
    string Path) : TerraformOptions
{
    [BooleanCommandSwitch("-force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("-ignore-remote-version")]
    public bool? IgnoreRemoteVersion { get; set; }
}
