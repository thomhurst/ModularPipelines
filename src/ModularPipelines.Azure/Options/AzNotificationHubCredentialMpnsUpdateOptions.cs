using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("notification-hub", "credential", "mpns", "update")]
public record AzNotificationHubCredentialMpnsUpdateOptions(
[property: CliOption("--certificate-key")] string CertificateKey,
[property: CliOption("--mpns-certificate")] string MpnsCertificate,
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;