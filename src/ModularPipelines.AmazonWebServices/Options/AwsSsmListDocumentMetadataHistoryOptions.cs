using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "list-document-metadata-history")]
public record AwsSsmListDocumentMetadataHistoryOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--metadata")] string Metadata
) : AwsOptions
{
    [CliOption("--document-version")]
    public string? DocumentVersion { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}