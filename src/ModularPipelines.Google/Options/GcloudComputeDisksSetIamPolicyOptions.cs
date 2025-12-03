using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "disks", "set-iam-policy")]
public record GcloudComputeDisksSetIamPolicyOptions(
[property: CliArgument] string Disk,
[property: CliArgument] string Zone,
[property: CliArgument] string PolicyFile
) : GcloudOptions;