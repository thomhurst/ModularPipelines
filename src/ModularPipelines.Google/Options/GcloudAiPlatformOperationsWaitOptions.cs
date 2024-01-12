using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai-platform", "operations", "wait")]
public record GcloudAiPlatformOperationsWaitOptions(
[property: PositionalArgument] string Operation
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}