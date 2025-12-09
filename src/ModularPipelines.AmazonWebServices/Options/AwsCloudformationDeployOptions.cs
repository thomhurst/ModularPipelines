using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "deploy")]
public record AwsCloudformationDeployOptions(
[property: CliOption("--template-file")] string TemplateFile,
[property: CliOption("--stack-name")] string StackName
) : AwsOptions
{
    [CliOption("--s3-bucket")]
    public string? S3Bucket { get; set; }

    [CliFlag("--force-upload")]
    public bool? ForceUpload { get; set; }

    [CliOption("--s3-prefix")]
    public string? S3Prefix { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--parameter-overrides")]
    public string? ParameterOverrides { get; set; }

    [CliOption("--capabilities")]
    public string[]? Capabilities { get; set; }

    [CliFlag("--no-execute-changeset")]
    public bool? NoExecuteChangeset { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--notification-arns")]
    public string[]? NotificationArns { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }
}