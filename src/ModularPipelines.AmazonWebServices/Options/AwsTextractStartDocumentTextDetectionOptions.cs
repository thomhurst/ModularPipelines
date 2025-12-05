using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("textract", "start-document-text-detection")]
public record AwsTextractStartDocumentTextDetectionOptions(
[property: CliOption("--document-location")] string DocumentLocation
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--job-tag")]
    public string? JobTag { get; set; }

    [CliOption("--notification-channel")]
    public string? NotificationChannel { get; set; }

    [CliOption("--output-config")]
    public string? OutputConfig { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}