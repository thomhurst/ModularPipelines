using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotfleetwise", "update-campaign")]
public record AwsIotfleetwiseUpdateCampaignOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--action")] string Action
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--data-extra-dimensions")]
    public string[]? DataExtraDimensions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}