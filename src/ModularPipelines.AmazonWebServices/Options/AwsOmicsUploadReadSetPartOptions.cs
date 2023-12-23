using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("omics", "upload-read-set-part")]
public record AwsOmicsUploadReadSetPartOptions(
[property: CommandSwitch("--sequence-store-id")] string SequenceStoreId,
[property: CommandSwitch("--upload-id")] string UploadId,
[property: CommandSwitch("--part-source")] string PartSource,
[property: CommandSwitch("--part-number")] int PartNumber,
[property: CommandSwitch("--payload")] string Payload
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}