using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "hmac", "delete")]
public record GcloudStorageHmacDeleteOptions(
[property: PositionalArgument] string AccessId
) : GcloudOptions;