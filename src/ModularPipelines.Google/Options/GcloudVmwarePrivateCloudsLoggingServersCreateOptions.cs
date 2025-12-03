using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds", "logging-servers", "create")]
public record GcloudVmwarePrivateCloudsLoggingServersCreateOptions(
[property: CliArgument] string LoggingServer,
[property: CliArgument] string Location,
[property: CliArgument] string PrivateCloud,
[property: CliOption("--hostname")] string Hostname,
[property: CliOption("--port")] string Port,
[property: CliOption("--protocol")] string Protocol,
[property: CliOption("--source-type")] string SourceType
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}