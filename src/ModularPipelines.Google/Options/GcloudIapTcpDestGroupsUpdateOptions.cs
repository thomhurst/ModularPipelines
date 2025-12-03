using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iap", "tcp", "dest-groups", "update")]
public record GcloudIapTcpDestGroupsUpdateOptions(
[property: CliArgument] string GroupName,
[property: CliOption("--region")] string Region,
[property: CliOption("--fqdn-list")] string FqdnList,
[property: CliOption("--ip-range-list")] string IpRangeList
) : GcloudOptions;