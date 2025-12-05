using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectcampaigns", "get-connect-instance-config")]
public record AwsConnectcampaignsGetConnectInstanceConfigOptions(
[property: CliOption("--connect-instance-id")] string ConnectInstanceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}