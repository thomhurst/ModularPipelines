using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize", "create-campaign")]
public record AwsPersonalizeCreateCampaignOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--solution-version-arn")] string SolutionVersionArn
) : AwsOptions
{
    [CliOption("--min-provisioned-tps")]
    public int? MinProvisionedTps { get; set; }

    [CliOption("--campaign-config")]
    public string? CampaignConfig { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}