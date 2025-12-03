using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces-web", "associate-user-settings")]
public record AwsWorkspacesWebAssociateUserSettingsOptions(
[property: CliOption("--portal-arn")] string PortalArn,
[property: CliOption("--user-settings-arn")] string UserSettingsArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}