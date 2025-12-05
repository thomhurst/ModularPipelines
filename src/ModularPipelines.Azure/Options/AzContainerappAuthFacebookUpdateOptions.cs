using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "auth", "facebook", "update")]
public record AzContainerappAuthFacebookUpdateOptions : AzOptions
{
    [CliOption("--app-id")]
    public string? AppId { get; set; }

    [CliOption("--app-secret")]
    public string? AppSecret { get; set; }

    [CliOption("--app-secret-name")]
    public string? AppSecretName { get; set; }

    [CliOption("--graph-api-version")]
    public string? GraphApiVersion { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--scopes")]
    public string? Scopes { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}