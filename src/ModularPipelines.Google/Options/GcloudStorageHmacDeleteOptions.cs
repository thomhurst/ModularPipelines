using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "hmac", "delete")]
public record GcloudStorageHmacDeleteOptions(
[property: CliArgument] string AccessId
) : GcloudOptions;