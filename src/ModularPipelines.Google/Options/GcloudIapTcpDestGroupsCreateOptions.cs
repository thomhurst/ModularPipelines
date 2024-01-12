using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iap", "tcp", "dest-groups", "create")]
public record GcloudIapTcpDestGroupsCreateOptions(
[property: PositionalArgument] string GroupName,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions
{
    [CommandSwitch("--fqdn-list")]
    public string? FqdnList { get; set; }

    [CommandSwitch("--ip-range-list")]
    public string? IpRangeList { get; set; }
}