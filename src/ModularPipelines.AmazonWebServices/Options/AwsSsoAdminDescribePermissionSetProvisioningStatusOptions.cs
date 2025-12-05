using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "describe-permission-set-provisioning-status")]
public record AwsSsoAdminDescribePermissionSetProvisioningStatusOptions(
[property: CliOption("--instance-arn")] string InstanceArn,
[property: CliOption("--provision-permission-set-request-id")] string ProvisionPermissionSetRequestId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}