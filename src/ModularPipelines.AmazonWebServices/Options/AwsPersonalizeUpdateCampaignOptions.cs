using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize", "update-campaign")]
public record AwsPersonalizeUpdateCampaignOptions(
[property: CommandSwitch("--campaign-arn")] string CampaignArn
) : AwsOptions
{
    [CommandSwitch("--solution-version-arn")]
    public string? SolutionVersionArn { get; set; }

    [CommandSwitch("--min-provisioned-tps")]
    public int? MinProvisionedTps { get; set; }

    [CommandSwitch("--campaign-config")]
    public string? CampaignConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}