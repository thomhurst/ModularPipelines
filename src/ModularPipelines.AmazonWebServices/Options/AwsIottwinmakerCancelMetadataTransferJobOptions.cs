using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iottwinmaker", "cancel-metadata-transfer-job")]
public record AwsIottwinmakerCancelMetadataTransferJobOptions(
[property: CliOption("--metadata-transfer-job-id")] string MetadataTransferJobId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}