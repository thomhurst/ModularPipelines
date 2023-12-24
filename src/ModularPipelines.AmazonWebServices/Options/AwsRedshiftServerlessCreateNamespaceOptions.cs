using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift-serverless", "create-namespace")]
public record AwsRedshiftServerlessCreateNamespaceOptions(
[property: CommandSwitch("--namespace-name")] string NamespaceName
) : AwsOptions
{
    [CommandSwitch("--admin-password-secret-kms-key-id")]
    public string? AdminPasswordSecretKmsKeyId { get; set; }

    [CommandSwitch("--admin-user-password")]
    public string? AdminUserPassword { get; set; }

    [CommandSwitch("--admin-username")]
    public string? AdminUsername { get; set; }

    [CommandSwitch("--db-name")]
    public string? DbName { get; set; }

    [CommandSwitch("--default-iam-role-arn")]
    public string? DefaultIamRoleArn { get; set; }

    [CommandSwitch("--iam-roles")]
    public string[]? IamRoles { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--log-exports")]
    public string[]? LogExports { get; set; }

    [CommandSwitch("--redshift-idc-application-arn")]
    public string? RedshiftIdcApplicationArn { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}