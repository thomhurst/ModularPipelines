using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize", "update-campaign")]
public record AwsPersonalizeUpdateCampaignOptions(
[property: CliOption("--campaign-arn")] string CampaignArn
) : AwsOptions
{
    [CliOption("--solution-version-arn")]
    public string? SolutionVersionArn { get; set; }

    [CliOption("--min-provisioned-tps")]
    public int? MinProvisionedTps { get; set; }

    [CliOption("--campaign-config")]
    public string? CampaignConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}