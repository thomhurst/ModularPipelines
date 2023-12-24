using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("comprehend", "start-topics-detection-job")]
public record AwsComprehendStartTopicsDetectionJobOptions(
[property: CommandSwitch("--input-data-config")] string InputDataConfig,
[property: CommandSwitch("--output-data-config")] string OutputDataConfig,
[property: CommandSwitch("--data-access-role-arn")] string DataAccessRoleArn
) : AwsOptions
{
    [CommandSwitch("--job-name")]
    public string? JobName { get; set; }

    [CommandSwitch("--number-of-topics")]
    public int? NumberOfTopics { get; set; }

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