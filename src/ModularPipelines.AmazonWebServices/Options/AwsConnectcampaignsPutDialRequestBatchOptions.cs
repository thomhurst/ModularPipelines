using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectcampaigns", "put-dial-request-batch")]
public record AwsConnectcampaignsPutDialRequestBatchOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--dial-requests")] string[] DialRequests
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}