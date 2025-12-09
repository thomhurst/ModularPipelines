using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "update-stack-set")]
public record AwsCloudformationUpdateStackSetOptions(
[property: CliOption("--stack-set-name")] string StackSetName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--template-body")]
    public string? TemplateBody { get; set; }

    [CliOption("--template-url")]
    public string? TemplateUrl { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--capabilities")]
    public string[]? Capabilities { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--operation-preferences")]
    public string? OperationPreferences { get; set; }

    [CliOption("--administration-role-arn")]
    public string? AdministrationRoleArn { get; set; }

    [CliOption("--execution-role-name")]
    public string? ExecutionRoleName { get; set; }

    [CliOption("--deployment-targets")]
    public string? DeploymentTargets { get; set; }

    [CliOption("--permission-model")]
    public string? PermissionModel { get; set; }

    [CliOption("--auto-deployment")]
    public string? AutoDeployment { get; set; }

    [CliOption("--operation-id")]
    public string? OperationId { get; set; }

    [CliOption("--accounts")]
    public string[]? Accounts { get; set; }

    [CliOption("--regions")]
    public string[]? Regions { get; set; }

    [CliOption("--call-as")]
    public string? CallAs { get; set; }

    [CliOption("--managed-execution")]
    public string? ManagedExecution { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}