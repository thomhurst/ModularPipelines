using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanroomsml", "list-audience-generation-jobs")]
public record AwsCleanroomsmlListAudienceGenerationJobsOptions : AwsOptions
{
    [CliOption("--collaboration-id")]
    public string? CollaborationId { get; set; }

    [CliOption("--configured-audience-model-arn")]
    public string? ConfiguredAudienceModelArn { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}