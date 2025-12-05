using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "dns-keys", "list")]
public record GcloudDnsDnsKeysListOptions(
[property: CliOption("--zone")] string Zone
) : GcloudOptions;