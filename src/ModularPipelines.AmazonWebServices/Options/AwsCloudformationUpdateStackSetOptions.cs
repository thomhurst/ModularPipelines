using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "update-stack-set")]
public record AwsCloudformationUpdateStackSetOptions(
[property: CommandSwitch("--stack-set-name")] string StackSetName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--template-body")]
    public string? TemplateBody { get; set; }

    [CommandSwitch("--template-url")]
    public string? TemplateUrl { get; set; }

    [CommandSwitch("--parameters")]
    public string[]? Parameters { get; set; }

    [CommandSwitch("--capabilities")]
    public string[]? Capabilities { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--operation-preferences")]
    public string? OperationPreferences { get; set; }

    [CommandSwitch("--administration-role-arn")]
    public string? AdministrationRoleArn { get; set; }

    [CommandSwitch("--execution-role-name")]
    public string? ExecutionRoleName { get; set; }

    [CommandSwitch("--deployment-targets")]
    public string? DeploymentTargets { get; set; }

    [CommandSwitch("--permission-model")]
    public string? PermissionModel { get; set; }

    [CommandSwitch("--auto-deployment")]
    public string? AutoDeployment { get; set; }

    [CommandSwitch("--operation-id")]
    public string? OperationId { get; set; }

    [CommandSwitch("--accounts")]
    public string[]? Accounts { get; set; }

    [CommandSwitch("--regions")]
    public string[]? Regions { get; set; }

    [CommandSwitch("--call-as")]
    public string? CallAs { get; set; }

    [CommandSwitch("--managed-execution")]
    public string? ManagedExecution { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}