using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("services", "api-keys", "describe")]
public record GcloudServicesApiKeysDescribeOptions(
[property: CliArgument] string Key,
[property: CliArgument] string Location
) : GcloudOptions;