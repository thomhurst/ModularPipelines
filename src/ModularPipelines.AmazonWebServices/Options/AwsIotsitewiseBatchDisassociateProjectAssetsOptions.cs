using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "batch-disassociate-project-assets")]
public record AwsIotsitewiseBatchDisassociateProjectAssetsOptions(
[property: CommandSwitch("--project-id")] string ProjectId,
[property: CommandSwitch("--asset-ids")] string[] AssetIds
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}