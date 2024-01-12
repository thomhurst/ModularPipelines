using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-clouds", "logging-servers", "describe")]
public record GcloudVmwarePrivateCloudsLoggingServersDescribeOptions(
[property: PositionalArgument] string LoggingServer,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PrivateCloud
) : GcloudOptions;