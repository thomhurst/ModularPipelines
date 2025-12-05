using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "batch-associate-project-assets")]
public record AwsIotsitewiseBatchAssociateProjectAssetsOptions(
[property: CliOption("--project-id")] string ProjectId,
[property: CliOption("--asset-ids")] string[] AssetIds
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}