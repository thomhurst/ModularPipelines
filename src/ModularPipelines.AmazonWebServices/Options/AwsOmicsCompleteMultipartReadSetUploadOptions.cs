using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("omics", "complete-multipart-read-set-upload")]
public record AwsOmicsCompleteMultipartReadSetUploadOptions(
[property: CommandSwitch("--sequence-store-id")] string SequenceStoreId,
[property: CommandSwitch("--upload-id")] string UploadId,
[property: CommandSwitch("--parts")] string[] Parts
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}