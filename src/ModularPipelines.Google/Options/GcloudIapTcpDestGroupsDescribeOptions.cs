using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iap", "tcp", "dest-groups", "describe")]
public record GcloudIapTcpDestGroupsDescribeOptions(
[property: PositionalArgument] string GroupName,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;