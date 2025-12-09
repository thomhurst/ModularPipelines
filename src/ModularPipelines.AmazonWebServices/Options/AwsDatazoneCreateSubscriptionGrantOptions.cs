using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "create-subscription-grant")]
public record AwsDatazoneCreateSubscriptionGrantOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--environment-identifier")] string EnvironmentIdentifier,
[property: CliOption("--granted-entity")] string GrantedEntity,
[property: CliOption("--subscription-target-identifier")] string SubscriptionTargetIdentifier
) : AwsOptions
{
    [CliOption("--asset-target-names")]
    public string[]? AssetTargetNames { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}