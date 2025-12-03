using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("service-directory", "endpoints", "describe")]
public record GcloudServiceDirectoryEndpointsDescribeOptions(
[property: CliArgument] string Endpoint,
[property: CliArgument] string Location,
[property: CliArgument] string Namespace,
[property: CliArgument] string Service
) : GcloudOptions;