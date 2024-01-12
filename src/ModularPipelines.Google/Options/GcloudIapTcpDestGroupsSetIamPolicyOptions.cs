using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iap", "tcp", "dest-groups", "set-iam-policy")]
public record GcloudIapTcpDestGroupsSetIamPolicyOptions(
[property: PositionalArgument] string PolicyFile,
[property: CommandSwitch("--dest-group")] string DestGroup,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;