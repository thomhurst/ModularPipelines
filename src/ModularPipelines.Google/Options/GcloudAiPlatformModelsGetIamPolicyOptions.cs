using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai-platform", "models", "get-iam-policy")]
public record GcloudAiPlatformModelsGetIamPolicyOptions(
[property: CliArgument] string Model
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}