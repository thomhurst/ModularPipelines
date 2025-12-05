using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "get-permissions-boundary-for-permission-set")]
public record AwsSsoAdminGetPermissionsBoundaryForPermissionSetOptions(
[property: CliOption("--instance-arn")] string InstanceArn,
[property: CliOption("--permission-set-arn")] string PermissionSetArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}