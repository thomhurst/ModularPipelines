using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("service-directory", "endpoints", "delete")]
public record GcloudServiceDirectoryEndpointsDeleteOptions(
[property: CliArgument] string Endpoint,
[property: CliArgument] string Location,
[property: CliArgument] string Namespace,
[property: CliArgument] string Service
) : GcloudOptions;