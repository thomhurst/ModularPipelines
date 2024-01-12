using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai-platform", "jobs", "cancel")]
public record GcloudAiPlatformJobsCancelOptions(
[property: PositionalArgument] string Job
) : GcloudOptions;