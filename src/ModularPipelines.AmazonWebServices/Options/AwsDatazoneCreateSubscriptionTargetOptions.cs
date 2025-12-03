using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "create-subscription-target")]
public record AwsDatazoneCreateSubscriptionTargetOptions(
[property: CliOption("--applicable-asset-types")] string[] ApplicableAssetTypes,
[property: CliOption("--authorized-principals")] string[] AuthorizedPrincipals,
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--environment-identifier")] string EnvironmentIdentifier,
[property: CliOption("--manage-access-role")] string ManageAccessRole,
[property: CliOption("--name")] string Name,
[property: CliOption("--subscription-target-config")] string[] SubscriptionTargetConfig,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--provider")]
    public string? Provider { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}