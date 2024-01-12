using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("config", "configurations", "create")]
public record GcloudConfigConfigurationsCreateOptions(
[property: PositionalArgument] string ConfigurationName
) : GcloudOptions
{
    [BooleanCommandSwitch("--activate")]
    public bool? Activate { get; set; }
}