using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("secrets", "locations", "describe")]
public record GcloudSecretsLocationsDescribeOptions(
[property: PositionalArgument] string Location
) : GcloudOptions;