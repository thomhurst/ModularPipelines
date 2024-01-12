using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai-platform", "models", "remove-iam-policy-binding")]
public record GcloudAiPlatformModelsRemoveIamPolicyBindingOptions(
[property: PositionalArgument] string Model,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}