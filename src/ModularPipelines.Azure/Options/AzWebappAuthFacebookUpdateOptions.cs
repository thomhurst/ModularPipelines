using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "auth", "facebook", "update")]
public record AzWebappAuthFacebookUpdateOptions : AzOptions
{
    [CommandSwitch("--app-id")]
    public string? AppId { get; set; }

    [CommandSwitch("--app-secret")]
    public string? AppSecret { get; set; }

    [CommandSwitch("--app-secret-setting-name")]
    public string? AppSecretSettingName { get; set; }

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

    [CommandSwitch("--slot")]
    public string? Slot { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}