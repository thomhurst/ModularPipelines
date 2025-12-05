using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "hmac", "create")]
public record GcloudStorageHmacCreateOptions(
[property: CliArgument] string ServiceAccount
) : GcloudOptions;