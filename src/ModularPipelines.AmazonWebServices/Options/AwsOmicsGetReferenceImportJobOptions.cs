using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("omics", "get-reference-import-job")]
public record AwsOmicsGetReferenceImportJobOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--reference-store-id")] string ReferenceStoreId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}