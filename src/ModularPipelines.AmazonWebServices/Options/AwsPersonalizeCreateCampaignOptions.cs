using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize", "create-campaign")]
public record AwsPersonalizeCreateCampaignOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--solution-version-arn")] string SolutionVersionArn
) : AwsOptions
{
    [CommandSwitch("--min-provisioned-tps")]
    public int? MinProvisionedTps { get; set; }

    [CommandSwitch("--campaign-config")]
    public string? CampaignConfig { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}