using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataexchange", "get-asset")]
public record AwsDataexchangeGetAssetOptions(
[property: CommandSwitch("--asset-id")] string AssetId,
[property: CommandSwitch("--data-set-id")] string DataSetId,
[property: CommandSwitch("--revision-id")] string RevisionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}