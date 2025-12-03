using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotfleetwise", "update-campaign")]
public record AwsIotfleetwiseUpdateCampaignOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--action")] string Action
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--data-extra-dimensions")]
    public string[]? DataExtraDimensions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}