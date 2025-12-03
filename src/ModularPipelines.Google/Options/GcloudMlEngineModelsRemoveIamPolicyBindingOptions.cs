using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml-engine", "models", "remove-iam-policy-binding")]
public record GcloudMlEngineModelsRemoveIamPolicyBindingOptions(
[property: CliArgument] string Model,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}