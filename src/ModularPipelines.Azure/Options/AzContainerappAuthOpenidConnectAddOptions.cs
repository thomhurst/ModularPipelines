using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "auth", "openid-connect", "add")]
public record AzContainerappAuthOpenidConnectAddOptions(
[property: CliOption("--provider-name")] string ProviderName
) : AzOptions
{
    [CliOption("--client-id")]
    public string? ClientId { get; set; }

    [CliOption("--client-secret")]
    public string? ClientSecret { get; set; }

    [CliOption("--client-secret-name")]
    public string? ClientSecretName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--openid-configuration")]
    public string? OpenidConfiguration { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--scopes")]
    public string? Scopes { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}