using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iap", "tcp", "dest-groups", "describe")]
public record GcloudIapTcpDestGroupsDescribeOptions(
[property: CliArgument] string GroupName,
[property: CliOption("--region")] string Region
) : GcloudOptions;