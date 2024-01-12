using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-clouds", "logging-servers", "create")]
public record GcloudVmwarePrivateCloudsLoggingServersCreateOptions(
[property: PositionalArgument] string LoggingServer,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PrivateCloud,
[property: CommandSwitch("--hostname")] string Hostname,
[property: CommandSwitch("--port")] string Port,
[property: CommandSwitch("--protocol")] string Protocol,
[property: CommandSwitch("--source-type")] string SourceType
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}