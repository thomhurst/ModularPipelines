using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "hmac", "create")]
public record GcloudStorageHmacCreateOptions(
[property: PositionalArgument] string ServiceAccount
) : GcloudOptions;