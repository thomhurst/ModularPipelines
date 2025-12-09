using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces-web", "disassociate-user-access-logging-settings")]
public record AwsWorkspacesWebDisassociateUserAccessLoggingSettingsOptions(
[property: CliOption("--portal-arn")] string PortalArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}