using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("omics", "abort-multipart-read-set-upload")]
public record AwsOmicsAbortMultipartReadSetUploadOptions(
[property: CliOption("--sequence-store-id")] string SequenceStoreId,
[property: CliOption("--upload-id")] string UploadId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}