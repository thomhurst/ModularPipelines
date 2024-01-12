using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "snapshot-settings", "describe")]
public record GcloudComputeSnapshotSettingsDescribeOptions : GcloudOptions;