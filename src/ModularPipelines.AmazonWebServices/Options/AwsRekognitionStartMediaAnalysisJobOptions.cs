using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "start-media-analysis-job")]
public record AwsRekognitionStartMediaAnalysisJobOptions(
[property: CliOption("--operations-config")] string OperationsConfig,
[property: CliOption("--input")] string Input,
[property: CliOption("--output-config")] string OutputConfig
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--job-name")]
    public string? JobName { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}