using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("endpoints", "services", "undelete")]
public record GcloudEndpointsServicesUndeleteOptions(
[property: CliArgument] string Service
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}