using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "update-subscription-target")]
public record AwsDatazoneUpdateSubscriptionTargetOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--environment-identifier")] string EnvironmentIdentifier,
[property: CliOption("--identifier")] string Identifier
) : AwsOptions
{
    [CliOption("--applicable-asset-types")]
    public string[]? ApplicableAssetTypes { get; set; }

    [CliOption("--authorized-principals")]
    public string[]? AuthorizedPrincipals { get; set; }

    [CliOption("--manage-access-role")]
    public string? ManageAccessRole { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--provider")]
    public string? Provider { get; set; }

    [CliOption("--subscription-target-config")]
    public string[]? SubscriptionTargetConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}