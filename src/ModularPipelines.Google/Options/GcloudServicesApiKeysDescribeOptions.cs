using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("services", "api-keys", "describe")]
public record GcloudServicesApiKeysDescribeOptions(
[property: PositionalArgument] string Key,
[property: PositionalArgument] string Location
) : GcloudOptions;