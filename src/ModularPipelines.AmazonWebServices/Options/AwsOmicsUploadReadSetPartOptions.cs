using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("omics", "upload-read-set-part")]
public record AwsOmicsUploadReadSetPartOptions(
[property: CliOption("--sequence-store-id")] string SequenceStoreId,
[property: CliOption("--upload-id")] string UploadId,
[property: CliOption("--part-source")] string PartSource,
[property: CliOption("--part-number")] int PartNumber,
[property: CliOption("--payload")] string Payload
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}