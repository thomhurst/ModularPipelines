using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "pause-contact")]
public record AwsConnectPauseContactOptions(
[property: CliOption("--contact-id")] string ContactId,
[property: CliOption("--instance-id")] string InstanceId
) : AwsOptions
{
    [CliOption("--contact-flow-id")]
    public string? ContactFlowId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}