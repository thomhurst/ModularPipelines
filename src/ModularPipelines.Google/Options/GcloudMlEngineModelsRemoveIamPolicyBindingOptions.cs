using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml-engine", "models", "remove-iam-policy-binding")]
public record GcloudMlEngineModelsRemoveIamPolicyBindingOptions(
[property: PositionalArgument] string Model,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}