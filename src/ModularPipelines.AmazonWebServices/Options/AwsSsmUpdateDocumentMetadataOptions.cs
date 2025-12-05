using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "update-document-metadata")]
public record AwsSsmUpdateDocumentMetadataOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--document-reviews")] string DocumentReviews
) : AwsOptions
{
    [CliOption("--document-version")]
    public string? DocumentVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}