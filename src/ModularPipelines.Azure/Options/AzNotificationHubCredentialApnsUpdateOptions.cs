using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("notification-hub", "credential", "apns", "update")]
public record AzNotificationHubCredentialApnsUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--apns-certificate")]
    public string? ApnsCertificate { get; set; }

    [CliOption("--app-id")]
    public string? AppId { get; set; }

    [CliOption("--app-name")]
    public string? AppName { get; set; }

    [CliOption("--certificate-key")]
    public string? CertificateKey { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--key-id")]
    public string? KeyId { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}