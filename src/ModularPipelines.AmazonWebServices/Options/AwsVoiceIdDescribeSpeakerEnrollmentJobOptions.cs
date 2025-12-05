using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("voice-id", "describe-speaker-enrollment-job")]
public record AwsVoiceIdDescribeSpeakerEnrollmentJobOptions(
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--job-id")] string JobId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}