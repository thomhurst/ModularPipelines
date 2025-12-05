using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "detach-managed-policy-from-permission-set")]
public record AwsSsoAdminDetachManagedPolicyFromPermissionSetOptions(
[property: CliOption("--instance-arn")] string InstanceArn,
[property: CliOption("--managed-policy-arn")] string ManagedPolicyArn,
[property: CliOption("--permission-set-arn")] string PermissionSetArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}