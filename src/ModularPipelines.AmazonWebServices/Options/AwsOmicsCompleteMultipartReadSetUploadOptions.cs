using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("omics", "complete-multipart-read-set-upload")]
public record AwsOmicsCompleteMultipartReadSetUploadOptions(
[property: CliOption("--sequence-store-id")] string SequenceStoreId,
[property: CliOption("--upload-id")] string UploadId,
[property: CliOption("--parts")] string[] Parts
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}