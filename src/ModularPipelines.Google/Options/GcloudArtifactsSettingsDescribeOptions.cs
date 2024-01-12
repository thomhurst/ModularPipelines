using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "settings", "describe")]
public record GcloudArtifactsSettingsDescribeOptions : GcloudOptions;