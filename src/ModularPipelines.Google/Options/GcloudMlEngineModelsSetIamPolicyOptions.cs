using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml-engine", "models", "set-iam-policy")]
public record GcloudMlEngineModelsSetIamPolicyOptions(
[property: PositionalArgument] string Model,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}