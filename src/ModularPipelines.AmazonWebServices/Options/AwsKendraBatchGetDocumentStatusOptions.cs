using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "batch-get-document-status")]
public record AwsKendraBatchGetDocumentStatusOptions(
[property: CliOption("--index-id")] string IndexId,
[property: CliOption("--document-info-list")] string[] DocumentInfoList
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}