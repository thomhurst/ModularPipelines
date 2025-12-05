using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces-web", "associate-user-access-logging-settings")]
public record AwsWorkspacesWebAssociateUserAccessLoggingSettingsOptions(
[property: CliOption("--portal-arn")] string PortalArn,
[property: CliOption("--user-access-logging-settings-arn")] string UserAccessLoggingSettingsArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}