using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "dns-keys", "list")]
public record GcloudDnsDnsKeysListOptions(
[property: CommandSwitch("--zone")] string Zone
) : GcloudOptions;