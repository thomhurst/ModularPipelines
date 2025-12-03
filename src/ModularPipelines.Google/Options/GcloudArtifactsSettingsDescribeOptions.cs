using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "settings", "describe")]
public record GcloudArtifactsSettingsDescribeOptions : GcloudOptions;