using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanroomsml", "get-audience-generation-job")]
public record AwsCleanroomsmlGetAudienceGenerationJobOptions(
[property: CliOption("--audience-generation-job-arn")] string AudienceGenerationJobArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}