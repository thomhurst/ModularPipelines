using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "snapshots", "get-iam-policy")]
public record GcloudComputeSnapshotsGetIamPolicyOptions(
[property: CliArgument] string SnapshotName
) : GcloudOptions;