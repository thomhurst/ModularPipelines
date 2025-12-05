using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iap", "tcp", "dest-groups", "create")]
public record GcloudIapTcpDestGroupsCreateOptions(
[property: CliArgument] string GroupName,
[property: CliOption("--region")] string Region
) : GcloudOptions
{
    [CliOption("--fqdn-list")]
    public string? FqdnList { get; set; }

    [CliOption("--ip-range-list")]
    public string? IpRangeList { get; set; }
}