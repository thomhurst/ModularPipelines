using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iap", "tcp", "dest-groups", "delete")]
public record GcloudIapTcpDestGroupsDeleteOptions(
[property: CliArgument] string GroupName,
[property: CliOption("--region")] string Region
) : GcloudOptions;