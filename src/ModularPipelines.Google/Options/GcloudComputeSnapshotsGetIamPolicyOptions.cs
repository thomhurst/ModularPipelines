using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "snapshots", "get-iam-policy")]
public record GcloudComputeSnapshotsGetIamPolicyOptions(
[property: PositionalArgument] string SnapshotName
) : GcloudOptions;