using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iap", "tcp", "dest-groups", "update")]
public record GcloudIapTcpDestGroupsUpdateOptions(
[property: PositionalArgument] string GroupName,
[property: CommandSwitch("--region")] string Region,
[property: CommandSwitch("--fqdn-list")] string FqdnList,
[property: CommandSwitch("--ip-range-list")] string IpRangeList
) : GcloudOptions;