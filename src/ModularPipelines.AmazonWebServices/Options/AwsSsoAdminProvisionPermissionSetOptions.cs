using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "provision-permission-set")]
public record AwsSsoAdminProvisionPermissionSetOptions(
[property: CliOption("--instance-arn")] string InstanceArn,
[property: CliOption("--permission-set-arn")] string PermissionSetArn,
[property: CliOption("--target-type")] string TargetType
) : AwsOptions
{
    [CliOption("--target-id")]
    public string? TargetId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}