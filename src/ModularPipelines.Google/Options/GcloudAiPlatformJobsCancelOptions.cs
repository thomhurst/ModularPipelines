using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai-platform", "jobs", "cancel")]
public record GcloudAiPlatformJobsCancelOptions(
[property: CliArgument] string Job
) : GcloudOptions;