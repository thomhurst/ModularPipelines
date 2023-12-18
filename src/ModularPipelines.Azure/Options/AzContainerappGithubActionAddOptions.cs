using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "github-action", "add")]
public record AzContainerappGithubActionAddOptions(
[property: CommandSwitch("--repo-url")] string RepoUrl
) : AzOptions
{
    [CommandSwitch("--branch")]
    public string? Branch { get; set; }

    [CommandSwitch("--context-path")]
    public string? ContextPath { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [BooleanCommandSwitch("--login-with-github")]
    public bool? LoginWithGithub { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--registry-password")]
    public string? RegistryPassword { get; set; }

    [CommandSwitch("--registry-url")]
    public string? RegistryUrl { get; set; }

    [CommandSwitch("--registry-username")]
    public string? RegistryUsername { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--service-principal-client-id")]
    public string? ServicePrincipalClientId { get; set; }

    [CommandSwitch("--service-principal-client-secret")]
    public string? ServicePrincipalClientSecret { get; set; }

    [CommandSwitch("--service-principal-tenant-id")]
    public string? ServicePrincipalTenantId { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}

