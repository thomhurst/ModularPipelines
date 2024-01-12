using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iap", "tcp", "dest-groups", "get-iam-policy")]
public record GcloudIapTcpDestGroupsGetIamPolicyOptions(
[property: CommandSwitch("--dest-group")] string DestGroup,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;