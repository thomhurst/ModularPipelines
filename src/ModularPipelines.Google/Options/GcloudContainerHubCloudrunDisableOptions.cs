using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "cloudrun", "disable")]
public record GcloudContainerHubCloudrunDisableOptions : GcloudOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }
}