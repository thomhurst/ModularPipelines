using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("comprehend", "start-document-classification-job")]
public record AwsComprehendStartDocumentClassificationJobOptions(
[property: CliOption("--input-data-config")] string InputDataConfig,
[property: CliOption("--output-data-config")] string OutputDataConfig,
[property: CliOption("--data-access-role-arn")] string DataAccessRoleArn
) : AwsOptions
{
    [CliOption("--job-name")]
    public string? JobName { get; set; }

    [CliOption("--document-classifier-arn")]
    public string? DocumentClassifierArn { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--volume-kms-key-id")]
    public string? VolumeKmsKeyId { get; set; }

    [CliOption("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--flywheel-arn")]
    public string? FlywheelArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}