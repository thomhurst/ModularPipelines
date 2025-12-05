using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iap", "tcp", "dest-groups", "get-iam-policy")]
public record GcloudIapTcpDestGroupsGetIamPolicyOptions(
[property: CliOption("--dest-group")] string DestGroup,
[property: CliOption("--region")] string Region
) : GcloudOptions;