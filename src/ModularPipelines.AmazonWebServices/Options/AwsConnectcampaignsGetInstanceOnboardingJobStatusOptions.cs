using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectcampaigns", "get-instance-onboarding-job-status")]
public record AwsConnectcampaignsGetInstanceOnboardingJobStatusOptions(
[property: CommandSwitch("--connect-instance-id")] string ConnectInstanceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}