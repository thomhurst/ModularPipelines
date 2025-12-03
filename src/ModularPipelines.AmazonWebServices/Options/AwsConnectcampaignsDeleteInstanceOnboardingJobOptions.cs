using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectcampaigns", "delete-instance-onboarding-job")]
public record AwsConnectcampaignsDeleteInstanceOnboardingJobOptions(
[property: CliOption("--connect-instance-id")] string ConnectInstanceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}