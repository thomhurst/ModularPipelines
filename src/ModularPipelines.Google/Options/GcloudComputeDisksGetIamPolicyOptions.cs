using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "disks", "get-iam-policy")]
public record GcloudComputeDisksGetIamPolicyOptions(
[property: PositionalArgument] string Disk,
[property: PositionalArgument] string Zone
) : GcloudOptions;