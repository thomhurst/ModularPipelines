using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "batch-put-document")]
public record AwsKendraBatchPutDocumentOptions(
[property: CliOption("--index-id")] string IndexId,
[property: CliOption("--documents")] string[] Documents
) : AwsOptions
{
    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--custom-document-enrichment-configuration")]
    public string? CustomDocumentEnrichmentConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}