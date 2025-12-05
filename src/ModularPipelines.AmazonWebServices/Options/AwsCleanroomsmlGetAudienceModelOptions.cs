using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanroomsml", "get-audience-model")]
public record AwsCleanroomsmlGetAudienceModelOptions(
[property: CliOption("--audience-model-arn")] string AudienceModelArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}