using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("comprehend", "start-targeted-sentiment-detection-job")]
public record AwsComprehendStartTargetedSentimentDetectionJobOptions(
[property: CommandSwitch("--input-data-config")] string InputDataConfig,
[property: CommandSwitch("--output-data-config")] string OutputDataConfig,
[property: CommandSwitch("--data-access-role-arn")] string DataAccessRoleArn,
[property: CommandSwitch("--language-code")] string LanguageCode
) : AwsOptions
{
    [CommandSwitch("--job-name")]
    public string? JobName { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--volume-kms-key-id")]
    public string? VolumeKmsKeyId { get; set; }

    [CommandSwitch("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}