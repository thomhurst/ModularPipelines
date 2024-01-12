using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai-platform", "models", "describe")]
public record GcloudAiPlatformModelsDescribeOptions(
[property: PositionalArgument] string Model
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}