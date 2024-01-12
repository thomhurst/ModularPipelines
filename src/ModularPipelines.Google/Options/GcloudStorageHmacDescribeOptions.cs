using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "hmac", "describe")]
public record GcloudStorageHmacDescribeOptions(
[property: PositionalArgument] string AccessId
) : GcloudOptions;