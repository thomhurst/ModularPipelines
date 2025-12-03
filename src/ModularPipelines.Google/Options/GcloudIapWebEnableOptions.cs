using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iap", "web", "enable")]
public record GcloudIapWebEnableOptions : GcloudOptions
{
    [CliOption("--oauth2-client-id")]
    public string? Oauth2ClientId { get; set; }

    [CliOption("--oauth2-client-secret")]
    public string? Oauth2ClientSecret { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }
}