using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("notification-hub", "credential", "apns", "update")]
public record AzNotificationHubCredentialApnsUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--apns-certificate")]
    public string? ApnsCertificate { get; set; }

    [CommandSwitch("--app-id")]
    public string? AppId { get; set; }

    [CommandSwitch("--app-name")]
    public string? AppName { get; set; }

    [CommandSwitch("--certificate-key")]
    public string? CertificateKey { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }

    [CommandSwitch("--key-id")]
    public string? KeyId { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}