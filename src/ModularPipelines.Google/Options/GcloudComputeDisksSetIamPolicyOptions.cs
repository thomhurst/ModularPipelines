using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "disks", "set-iam-policy")]
public record GcloudComputeDisksSetIamPolicyOptions(
[property: PositionalArgument] string Disk,
[property: PositionalArgument] string Zone,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;