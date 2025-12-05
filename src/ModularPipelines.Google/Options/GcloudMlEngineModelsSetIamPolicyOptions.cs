using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml-engine", "models", "set-iam-policy")]
public record GcloudMlEngineModelsSetIamPolicyOptions(
[property: CliArgument] string Model,
[property: CliArgument] string PolicyFile
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}