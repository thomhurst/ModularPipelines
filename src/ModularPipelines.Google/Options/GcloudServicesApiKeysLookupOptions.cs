using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("services", "api-keys", "lookup")]
public record GcloudServicesApiKeysLookupOptions(
[property: CliArgument] string KeyString
) : GcloudOptions;