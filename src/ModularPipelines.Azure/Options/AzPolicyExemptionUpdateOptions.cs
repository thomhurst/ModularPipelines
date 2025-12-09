using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("policy", "exemption", "update")]
public record AzPolicyExemptionUpdateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--exemption-category")]
    public string? ExemptionCategory { get; set; }

    [CliOption("--expires-on")]
    public string? ExpiresOn { get; set; }

    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliOption("--policy-definition-reference-ids")]
    public string? PolicyDefinitionReferenceIds { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }
}