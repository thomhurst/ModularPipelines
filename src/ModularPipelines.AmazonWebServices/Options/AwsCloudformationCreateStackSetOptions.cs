using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "create-stack-set")]
public record AwsCloudformationCreateStackSetOptions(
[property: CliOption("--stack-set-name")] string StackSetName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--template-body")]
    public string? TemplateBody { get; set; }

    [CliOption("--template-url")]
    public string? TemplateUrl { get; set; }

    [CliOption("--stack-id")]
    public string? StackId { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--capabilities")]
    public string[]? Capabilities { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--administration-role-arn")]
    public string? AdministrationRoleArn { get; set; }

    [CliOption("--execution-role-name")]
    public string? ExecutionRoleName { get; set; }

    [CliOption("--permission-model")]
    public string? PermissionModel { get; set; }

    [CliOption("--auto-deployment")]
    public string? AutoDeployment { get; set; }

    [CliOption("--call-as")]
    public string? CallAs { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--managed-execution")]
    public string? ManagedExecution { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}