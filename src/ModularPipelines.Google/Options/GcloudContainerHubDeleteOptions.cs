using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "delete")]
public record GcloudContainerHubDeleteOptions : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}