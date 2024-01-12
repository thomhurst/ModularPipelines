using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "dns-keys", "describe")]
public record GcloudDnsDnsKeysDescribeOptions(
[property: PositionalArgument] string KeyId,
[property: CommandSwitch("--zone")] string Zone
) : GcloudOptions;