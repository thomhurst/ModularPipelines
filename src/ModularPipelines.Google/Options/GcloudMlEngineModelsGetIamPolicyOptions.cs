using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml-engine", "models", "get-iam-policy")]
public record GcloudMlEngineModelsGetIamPolicyOptions(
[property: PositionalArgument] string Model
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}