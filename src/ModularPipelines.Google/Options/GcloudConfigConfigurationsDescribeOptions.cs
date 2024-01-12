using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("config", "configurations", "describe")]
public record GcloudConfigConfigurationsDescribeOptions(
[property: PositionalArgument] string ConfigurationName
) : GcloudOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }
}