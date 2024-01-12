using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("services", "api-keys", "get-key-string")]
public record GcloudServicesApiKeysGetKeyStringOptions(
[property: PositionalArgument] string Key,
[property: PositionalArgument] string Location
) : GcloudOptions;