using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanroomsml", "put-configured-audience-model-policy")]
public record AwsCleanroomsmlPutConfiguredAudienceModelPolicyOptions(
[property: CliOption("--configured-audience-model-arn")] string ConfiguredAudienceModelArn,
[property: CliOption("--configured-audience-model-policy")] string ConfiguredAudienceModelPolicy
) : AwsOptions
{
    [CliOption("--policy-existence-condition")]
    public string? PolicyExistenceCondition { get; set; }

    [CliOption("--previous-policy-hash")]
    public string? PreviousPolicyHash { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}