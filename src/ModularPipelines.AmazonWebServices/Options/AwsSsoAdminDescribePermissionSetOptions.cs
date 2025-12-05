using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "describe-permission-set")]
public record AwsSsoAdminDescribePermissionSetOptions(
[property: CliOption("--instance-arn")] string InstanceArn,
[property: CliOption("--permission-set-arn")] string PermissionSetArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}