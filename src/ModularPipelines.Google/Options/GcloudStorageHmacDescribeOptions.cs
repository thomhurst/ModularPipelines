using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "hmac", "describe")]
public record GcloudStorageHmacDescribeOptions(
[property: CliArgument] string AccessId
) : GcloudOptions;