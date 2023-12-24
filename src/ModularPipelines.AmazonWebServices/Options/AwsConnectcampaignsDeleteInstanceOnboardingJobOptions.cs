using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectcampaigns", "delete-instance-onboarding-job")]
public record AwsConnectcampaignsDeleteInstanceOnboardingJobOptions(
[property: CommandSwitch("--connect-instance-id")] string ConnectInstanceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}