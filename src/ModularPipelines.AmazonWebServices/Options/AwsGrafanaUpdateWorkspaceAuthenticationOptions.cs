using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("grafana", "update-workspace-authentication")]
public record AwsGrafanaUpdateWorkspaceAuthenticationOptions(
[property: CliOption("--authentication-providers")] string[] AuthenticationProviders,
[property: CliOption("--workspace-id")] string WorkspaceId
) : AwsOptions
{
    [CliOption("--saml-configuration")]
    public string? SamlConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}