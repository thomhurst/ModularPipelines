using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("services", "api-keys", "lookup")]
public record GcloudServicesApiKeysLookupOptions(
[property: PositionalArgument] string KeyString
) : GcloudOptions;