using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai-platform", "models", "get-iam-policy")]
public record GcloudAiPlatformModelsGetIamPolicyOptions(
[property: PositionalArgument] string Model
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}