using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanroomsml", "start-audience-generation-job")]
public record AwsCleanroomsmlStartAudienceGenerationJobOptions(
[property: CliOption("--configured-audience-model-arn")] string ConfiguredAudienceModelArn,
[property: CliOption("--name")] string Name,
[property: CliOption("--seed-audience")] string SeedAudience
) : AwsOptions
{
    [CliOption("--collaboration-id")]
    public string? CollaborationId { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}