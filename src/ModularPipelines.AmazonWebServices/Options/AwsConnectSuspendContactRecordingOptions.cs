using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "suspend-contact-recording")]
public record AwsConnectSuspendContactRecordingOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--contact-id")] string ContactId,
[property: CliOption("--initial-contact-id")] string InitialContactId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}