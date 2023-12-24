using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanroomsml", "put-configured-audience-model-policy")]
public record AwsCleanroomsmlPutConfiguredAudienceModelPolicyOptions(
[property: CommandSwitch("--configured-audience-model-arn")] string ConfiguredAudienceModelArn,
[property: CommandSwitch("--configured-audience-model-policy")] string ConfiguredAudienceModelPolicy
) : AwsOptions
{
    [CommandSwitch("--policy-existence-condition")]
    public string? PolicyExistenceCondition { get; set; }

    [CommandSwitch("--previous-policy-hash")]
    public string? PreviousPolicyHash { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}