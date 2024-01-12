using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai-platform", "versions", "list")]
public record GcloudAiPlatformVersionsListOptions(
[property: CommandSwitch("--model")] string Model
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}