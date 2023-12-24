using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces-web", "update-portal")]
public record AwsWorkspacesWebUpdatePortalOptions(
[property: CommandSwitch("--portal-arn")] string PortalArn
) : AwsOptions
{
    [CommandSwitch("--authentication-type")]
    public string? AuthenticationType { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}