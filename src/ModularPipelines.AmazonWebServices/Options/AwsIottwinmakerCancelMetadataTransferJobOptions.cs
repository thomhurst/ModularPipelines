using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iottwinmaker", "cancel-metadata-transfer-job")]
public record AwsIottwinmakerCancelMetadataTransferJobOptions(
[property: CommandSwitch("--metadata-transfer-job-id")] string MetadataTransferJobId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}