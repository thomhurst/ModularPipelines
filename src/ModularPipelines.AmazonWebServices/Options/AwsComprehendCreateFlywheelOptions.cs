using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("comprehend", "create-flywheel")]
public record AwsComprehendCreateFlywheelOptions(
[property: CliOption("--flywheel-name")] string FlywheelName,
[property: CliOption("--data-access-role-arn")] string DataAccessRoleArn,
[property: CliOption("--data-lake-s3-uri")] string DataLakeS3Uri
) : AwsOptions
{
    [CliOption("--active-model-arn")]
    public string? ActiveModelArn { get; set; }

    [CliOption("--task-config")]
    public string? TaskConfig { get; set; }

    [CliOption("--model-type")]
    public string? ModelType { get; set; }

    [CliOption("--data-security-config")]
    public string? DataSecurityConfig { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}