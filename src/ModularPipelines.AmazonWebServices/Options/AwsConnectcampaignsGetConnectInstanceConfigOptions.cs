using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectcampaigns", "get-connect-instance-config")]
public record AwsConnectcampaignsGetConnectInstanceConfigOptions(
[property: CommandSwitch("--connect-instance-id")] string ConnectInstanceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}