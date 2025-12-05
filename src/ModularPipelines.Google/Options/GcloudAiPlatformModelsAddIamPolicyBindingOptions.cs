using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai-platform", "models", "add-iam-policy-binding")]
public record GcloudAiPlatformModelsAddIamPolicyBindingOptions(
[property: CliArgument] string Model,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}