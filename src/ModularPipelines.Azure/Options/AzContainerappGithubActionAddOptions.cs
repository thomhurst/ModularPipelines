using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "github-action", "add")]
public record AzContainerappGithubActionAddOptions(
[property: CliOption("--repo-url")] string RepoUrl
) : AzOptions
{
    [CliOption("--branch")]
    public string? Branch { get; set; }

    [CliOption("--context-path")]
    public string? ContextPath { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliFlag("--login-with-github")]
    public bool? LoginWithGithub { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--registry-password")]
    public string? RegistryPassword { get; set; }

    [CliOption("--registry-url")]
    public string? RegistryUrl { get; set; }

    [CliOption("--registry-username")]
    public string? RegistryUsername { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--service-principal-client-id")]
    public string? ServicePrincipalClientId { get; set; }

    [CliOption("--service-principal-client-secret")]
    public string? ServicePrincipalClientSecret { get; set; }

    [CliOption("--service-principal-tenant-id")]
    public string? ServicePrincipalTenantId { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}