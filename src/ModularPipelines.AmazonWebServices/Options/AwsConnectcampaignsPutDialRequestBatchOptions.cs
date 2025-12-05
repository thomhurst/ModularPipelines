using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectcampaigns", "put-dial-request-batch")]
public record AwsConnectcampaignsPutDialRequestBatchOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--dial-requests")] string[] DialRequests
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}