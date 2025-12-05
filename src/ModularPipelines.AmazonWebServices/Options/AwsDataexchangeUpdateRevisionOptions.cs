using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataexchange", "update-revision")]
public record AwsDataexchangeUpdateRevisionOptions(
[property: CliOption("--data-set-id")] string DataSetId,
[property: CliOption("--revision-id")] string RevisionId
) : AwsOptions
{
    [CliOption("--comment")]
    public string? Comment { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}