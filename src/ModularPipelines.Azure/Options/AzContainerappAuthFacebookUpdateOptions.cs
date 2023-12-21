using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "auth", "facebook", "update")]
public record AzContainerappAuthFacebookUpdateOptions : AzOptions
{
    [CommandSwitch("--app-id")]
    public string? AppId { get; set; }

    [CommandSwitch("--app-secret")]
    public string? AppSecret { get; set; }

    [CommandSwitch("--app-secret-name")]
    public string? AppSecretName { get; set; }

    [CommandSwitch("--graph-api-version")]
    public string? GraphApiVersion { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--scopes")]
    public string? Scopes { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}