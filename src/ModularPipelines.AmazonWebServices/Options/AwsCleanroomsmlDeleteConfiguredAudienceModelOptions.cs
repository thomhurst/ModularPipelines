using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanroomsml", "delete-configured-audience-model")]
public record AwsCleanroomsmlDeleteConfiguredAudienceModelOptions(
[property: CliOption("--configured-audience-model-arn")] string ConfiguredAudienceModelArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}