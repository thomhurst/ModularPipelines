using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("services", "api-keys", "get-key-string")]
public record GcloudServicesApiKeysGetKeyStringOptions(
[property: CliArgument] string Key,
[property: CliArgument] string Location
) : GcloudOptions;