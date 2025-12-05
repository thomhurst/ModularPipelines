using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "start-contact-recording")]
public record AwsConnectStartContactRecordingOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--contact-id")] string ContactId,
[property: CliOption("--initial-contact-id")] string InitialContactId,
[property: CliOption("--voice-recording-configuration")] string VoiceRecordingConfiguration
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}