using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iap", "tcp", "dest-groups", "delete")]
public record GcloudIapTcpDestGroupsDeleteOptions(
[property: PositionalArgument] string GroupName,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;