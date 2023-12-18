using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("notification-hub", "credential", "mpns", "update")]
public record AzNotificationHubCredentialMpnsUpdateOptions(
[property: CommandSwitch("--certificate-key")] string CertificateKey,
[property: CommandSwitch("--mpns-certificate")] string MpnsCertificate,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;