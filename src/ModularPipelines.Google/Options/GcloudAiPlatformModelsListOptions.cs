using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai-platform", "models", "list")]
public record GcloudAiPlatformModelsListOptions : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}