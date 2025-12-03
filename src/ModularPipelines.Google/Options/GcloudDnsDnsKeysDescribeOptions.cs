using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "dns-keys", "describe")]
public record GcloudDnsDnsKeysDescribeOptions(
[property: CliArgument] string KeyId,
[property: CliOption("--zone")] string Zone
) : GcloudOptions;