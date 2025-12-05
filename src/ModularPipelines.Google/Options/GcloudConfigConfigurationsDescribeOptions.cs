using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("config", "configurations", "describe")]
public record GcloudConfigConfigurationsDescribeOptions(
[property: CliArgument] string ConfigurationName
) : GcloudOptions
{
    [CliFlag("--all")]
    public bool? All { get; set; }
}