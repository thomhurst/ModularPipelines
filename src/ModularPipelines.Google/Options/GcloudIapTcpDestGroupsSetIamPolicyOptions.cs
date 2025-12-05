using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iap", "tcp", "dest-groups", "set-iam-policy")]
public record GcloudIapTcpDestGroupsSetIamPolicyOptions(
[property: CliArgument] string PolicyFile,
[property: CliOption("--dest-group")] string DestGroup,
[property: CliOption("--region")] string Region
) : GcloudOptions;