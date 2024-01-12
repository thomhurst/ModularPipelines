using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "snapshots", "set-iam-policy")]
public record GcloudComputeSnapshotsSetIamPolicyOptions(
[property: PositionalArgument] string SnapshotName,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;