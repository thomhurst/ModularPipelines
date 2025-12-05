using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "update-permission-set")]
public record AwsSsoAdminUpdatePermissionSetOptions(
[property: CliOption("--instance-arn")] string InstanceArn,
[property: CliOption("--permission-set-arn")] string PermissionSetArn
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--relay-state")]
    public string? RelayState { get; set; }

    [CliOption("--session-duration")]
    public string? SessionDuration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}