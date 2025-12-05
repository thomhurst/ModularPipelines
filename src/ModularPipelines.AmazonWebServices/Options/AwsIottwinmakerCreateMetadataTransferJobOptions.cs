using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iottwinmaker", "create-metadata-transfer-job")]
public record AwsIottwinmakerCreateMetadataTransferJobOptions(
[property: CliOption("--sources")] string[] Sources,
[property: CliOption("--destination")] string Destination
) : AwsOptions
{
    [CliOption("--metadata-transfer-job-id")]
    public string? MetadataTransferJobId { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}