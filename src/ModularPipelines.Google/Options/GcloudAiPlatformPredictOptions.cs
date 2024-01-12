using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai-platform", "predict")]
public record GcloudAiPlatformPredictOptions(
[property: CommandSwitch("--model")] string Model,
[property: CommandSwitch("--json-instances")] string JsonInstances,
[property: CommandSwitch("--json-request")] string JsonRequest,
[property: CommandSwitch("--text-instances")] string TextInstances
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--signature-name")]
    public string? SignatureName { get; set; }

    [CommandSwitch("--version")]
    public new string? Version { get; set; }
}