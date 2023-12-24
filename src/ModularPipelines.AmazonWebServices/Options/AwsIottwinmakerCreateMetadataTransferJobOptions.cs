using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iottwinmaker", "create-metadata-transfer-job")]
public record AwsIottwinmakerCreateMetadataTransferJobOptions(
[property: CommandSwitch("--sources")] string[] Sources,
[property: CommandSwitch("--destination")] string Destination
) : AwsOptions
{
    [CommandSwitch("--metadata-transfer-job-id")]
    public string? MetadataTransferJobId { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}