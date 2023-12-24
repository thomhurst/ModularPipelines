using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanroomsml", "delete-configured-audience-model-policy")]
public record AwsCleanroomsmlDeleteConfiguredAudienceModelPolicyOptions(
[property: CommandSwitch("--configured-audience-model-arn")] string ConfiguredAudienceModelArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}