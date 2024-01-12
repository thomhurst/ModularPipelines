using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai-platform", "operations", "cancel")]
public record GcloudAiPlatformOperationsCancelOptions(
[property: PositionalArgument] string Operation
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}