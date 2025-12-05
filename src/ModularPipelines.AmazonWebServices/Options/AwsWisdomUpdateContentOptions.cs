using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wisdom", "update-content")]
public record AwsWisdomUpdateContentOptions(
[property: CliOption("--content-id")] string ContentId,
[property: CliOption("--knowledge-base-id")] string KnowledgeBaseId
) : AwsOptions
{
    [CliOption("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CliOption("--override-link-out-uri")]
    public string? OverrideLinkOutUri { get; set; }

    [CliOption("--revision-id")]
    public string? RevisionId { get; set; }

    [CliOption("--title")]
    public string? Title { get; set; }

    [CliOption("--upload-id")]
    public string? UploadId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}