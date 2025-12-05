using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "disks", "get-iam-policy")]
public record GcloudComputeDisksGetIamPolicyOptions(
[property: CliArgument] string Disk,
[property: CliArgument] string Zone
) : GcloudOptions;