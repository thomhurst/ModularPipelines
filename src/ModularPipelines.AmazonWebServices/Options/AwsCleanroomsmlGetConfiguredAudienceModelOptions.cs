using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanroomsml", "get-configured-audience-model")]
public record AwsCleanroomsmlGetConfiguredAudienceModelOptions(
[property: CliOption("--configured-audience-model-arn")] string ConfiguredAudienceModelArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}