using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secrets", "locations", "describe")]
public record GcloudSecretsLocationsDescribeOptions(
[property: CliArgument] string Location
) : GcloudOptions;