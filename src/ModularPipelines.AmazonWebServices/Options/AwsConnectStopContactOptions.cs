using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "stop-contact")]
public record AwsConnectStopContactOptions(
[property: CliOption("--contact-id")] string ContactId,
[property: CliOption("--instance-id")] string InstanceId
) : AwsOptions
{
    [CliOption("--disconnect-reason")]
    public string? DisconnectReason { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}