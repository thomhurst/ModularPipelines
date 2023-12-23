using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("grafana", "update-workspace-authentication")]
public record AwsGrafanaUpdateWorkspaceAuthenticationOptions(
[property: CommandSwitch("--authentication-providers")] string[] AuthenticationProviders,
[property: CommandSwitch("--workspace-id")] string WorkspaceId
) : AwsOptions
{
    [CommandSwitch("--saml-configuration")]
    public string? SamlConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}