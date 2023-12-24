using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "start-media-analysis-job")]
public record AwsRekognitionStartMediaAnalysisJobOptions(
[property: CommandSwitch("--operations-config")] string OperationsConfig,
[property: CommandSwitch("--input")] string Input,
[property: CommandSwitch("--output-config")] string OutputConfig
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--job-name")]
    public string? JobName { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}