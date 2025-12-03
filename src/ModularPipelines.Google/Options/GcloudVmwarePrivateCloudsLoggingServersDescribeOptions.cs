using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds", "logging-servers", "describe")]
public record GcloudVmwarePrivateCloudsLoggingServersDescribeOptions(
[property: CliArgument] string LoggingServer,
[property: CliArgument] string Location,
[property: CliArgument] string PrivateCloud
) : GcloudOptions;