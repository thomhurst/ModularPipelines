using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds", "logging-servers", "update")]
public record GcloudVmwarePrivateCloudsLoggingServersUpdateOptions(
[property: CliArgument] string LoggingServer,
[property: CliArgument] string Location,
[property: CliArgument] string PrivateCloud
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--hostname")]
    public string? Hostname { get; set; }

    [CliOption("--port")]
    public string? Port { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--source-type")]
    public string? SourceType { get; set; }
}