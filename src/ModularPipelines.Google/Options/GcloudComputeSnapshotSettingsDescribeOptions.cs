using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "snapshot-settings", "describe")]
public record GcloudComputeSnapshotSettingsDescribeOptions : GcloudOptions;