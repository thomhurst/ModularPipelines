using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanroomsml", "delete-audience-generation-job")]
public record AwsCleanroomsmlDeleteAudienceGenerationJobOptions(
[property: CliOption("--audience-generation-job-arn")] string AudienceGenerationJobArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}