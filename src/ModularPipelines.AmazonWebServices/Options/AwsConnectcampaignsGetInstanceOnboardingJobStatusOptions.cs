using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectcampaigns", "get-instance-onboarding-job-status")]
public record AwsConnectcampaignsGetInstanceOnboardingJobStatusOptions(
[property: CliOption("--connect-instance-id")] string ConnectInstanceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}