using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectcampaigns", "start-instance-onboarding-job")]
public record AwsConnectcampaignsStartInstanceOnboardingJobOptions(
[property: CommandSwitch("--connect-instance-id")] string ConnectInstanceId,
[property: CommandSwitch("--encryption-config")] string EncryptionConfig
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}