using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "deploy")]
public record AwsCloudformationDeployOptions(
[property: CommandSwitch("--template-file")] string TemplateFile,
[property: CommandSwitch("--stack-name")] string StackName
) : AwsOptions
{
    [CommandSwitch("--s3-bucket")]
    public string? S3Bucket { get; set; }

    [BooleanCommandSwitch("--force-upload")]
    public bool? ForceUpload { get; set; }

    [CommandSwitch("--s3-prefix")]
    public string? S3Prefix { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--parameter-overrides")]
    public string? ParameterOverrides { get; set; }

    [CommandSwitch("--capabilities")]
    public string[]? Capabilities { get; set; }

    [BooleanCommandSwitch("--no-execute-changeset")]
    public bool? NoExecuteChangeset { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--notification-arns")]
    public string[]? NotificationArns { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }
}