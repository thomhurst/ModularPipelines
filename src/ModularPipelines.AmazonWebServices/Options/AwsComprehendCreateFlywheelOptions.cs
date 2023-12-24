using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("comprehend", "create-flywheel")]
public record AwsComprehendCreateFlywheelOptions(
[property: CommandSwitch("--flywheel-name")] string FlywheelName,
[property: CommandSwitch("--data-access-role-arn")] string DataAccessRoleArn,
[property: CommandSwitch("--data-lake-s3-uri")] string DataLakeS3Uri
) : AwsOptions
{
    [CommandSwitch("--active-model-arn")]
    public string? ActiveModelArn { get; set; }

    [CommandSwitch("--task-config")]
    public string? TaskConfig { get; set; }

    [CommandSwitch("--model-type")]
    public string? ModelType { get; set; }

    [CommandSwitch("--data-security-config")]
    public string? DataSecurityConfig { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}