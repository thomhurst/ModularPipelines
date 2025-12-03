using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds", "logging-servers", "delete")]
public record GcloudVmwarePrivateCloudsLoggingServersDeleteOptions(
[property: CliArgument] string LoggingServer,
[property: CliArgument] string Location,
[property: CliArgument] string PrivateCloud
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}