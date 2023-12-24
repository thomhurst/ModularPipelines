using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataexchange", "update-asset")]
public record AwsDataexchangeUpdateAssetOptions(
[property: CommandSwitch("--asset-id")] string AssetId,
[property: CommandSwitch("--data-set-id")] string DataSetId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--revision-id")] string RevisionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}