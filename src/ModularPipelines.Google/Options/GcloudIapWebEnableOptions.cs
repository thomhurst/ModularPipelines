using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iap", "web", "enable")]
public record GcloudIapWebEnableOptions : GcloudOptions
{
    [CommandSwitch("--oauth2-client-id")]
    public string? Oauth2ClientId { get; set; }

    [CommandSwitch("--oauth2-client-secret")]
    public string? Oauth2ClientSecret { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--service")]
    public string? Service { get; set; }
}