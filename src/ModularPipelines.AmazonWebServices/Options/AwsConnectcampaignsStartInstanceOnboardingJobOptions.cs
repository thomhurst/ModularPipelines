using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectcampaigns", "start-instance-onboarding-job")]
public record AwsConnectcampaignsStartInstanceOnboardingJobOptions(
[property: CliOption("--connect-instance-id")] string ConnectInstanceId,
[property: CliOption("--encryption-config")] string EncryptionConfig
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}