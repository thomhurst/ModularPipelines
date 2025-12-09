using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "transfer-contact")]
public record AwsConnectTransferContactOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--contact-id")] string ContactId,
[property: CliOption("--contact-flow-id")] string ContactFlowId
) : AwsOptions
{
    [CliOption("--queue-id")]
    public string? QueueId { get; set; }

    [CliOption("--user-id")]
    public string? UserId { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}