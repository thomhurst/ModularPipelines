using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "attach-managed-policy-to-permission-set")]
public record AwsSsoAdminAttachManagedPolicyToPermissionSetOptions(
[property: CliOption("--instance-arn")] string InstanceArn,
[property: CliOption("--managed-policy-arn")] string ManagedPolicyArn,
[property: CliOption("--permission-set-arn")] string PermissionSetArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}