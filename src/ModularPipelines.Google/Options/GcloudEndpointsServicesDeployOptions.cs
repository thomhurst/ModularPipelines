using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("endpoints", "services", "deploy")]
public record GcloudEndpointsServicesDeployOptions(
[property: CliArgument] string ServiceConfigFile
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }
}