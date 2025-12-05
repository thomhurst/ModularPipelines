using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectcampaigns", "delete-connect-instance-config")]
public record AwsConnectcampaignsDeleteConnectInstanceConfigOptions(
[property: CliOption("--connect-instance-id")] string ConnectInstanceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}