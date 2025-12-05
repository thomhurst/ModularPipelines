using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("config", "configurations", "create")]
public record GcloudConfigConfigurationsCreateOptions(
[property: CliArgument] string ConfigurationName
) : GcloudOptions
{
    [CliFlag("--activate")]
    public bool? Activate { get; set; }
}