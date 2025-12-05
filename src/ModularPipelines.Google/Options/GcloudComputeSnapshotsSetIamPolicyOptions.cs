using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "snapshots", "set-iam-policy")]
public record GcloudComputeSnapshotsSetIamPolicyOptions(
[property: CliArgument] string SnapshotName,
[property: CliArgument] string PolicyFile
) : GcloudOptions;