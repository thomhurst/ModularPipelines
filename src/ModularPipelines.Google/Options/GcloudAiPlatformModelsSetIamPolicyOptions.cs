using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai-platform", "models", "set-iam-policy")]
public record GcloudAiPlatformModelsSetIamPolicyOptions(
[property: CliArgument] string Model,
[property: CliArgument] string PolicyFile
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}