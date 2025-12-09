using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift-serverless", "update-namespace")]
public record AwsRedshiftServerlessUpdateNamespaceOptions(
[property: CliOption("--namespace-name")] string NamespaceName
) : AwsOptions
{
    [CliOption("--admin-password-secret-kms-key-id")]
    public string? AdminPasswordSecretKmsKeyId { get; set; }

    [CliOption("--admin-user-password")]
    public string? AdminUserPassword { get; set; }

    [CliOption("--admin-username")]
    public string? AdminUsername { get; set; }

    [CliOption("--default-iam-role-arn")]
    public string? DefaultIamRoleArn { get; set; }

    [CliOption("--iam-roles")]
    public string[]? IamRoles { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--log-exports")]
    public string[]? LogExports { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}