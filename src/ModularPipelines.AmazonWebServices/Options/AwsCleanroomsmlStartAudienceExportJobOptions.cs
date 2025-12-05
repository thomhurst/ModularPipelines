using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanroomsml", "start-audience-export-job")]
public record AwsCleanroomsmlStartAudienceExportJobOptions(
[property: CliOption("--audience-generation-job-arn")] string AudienceGenerationJobArn,
[property: CliOption("--audience-size")] string AudienceSize,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}