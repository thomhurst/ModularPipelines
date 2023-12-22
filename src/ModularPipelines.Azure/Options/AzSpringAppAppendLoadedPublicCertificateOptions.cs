using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "app", "append-loaded-public-certificate")]
public record AzSpringAppAppendLoadedPublicCertificateOptions(
[property: CommandSwitch("--certificate-name")] string CertificateName,
[property: BooleanCommandSwitch("--load-trust-store")] bool LoadTrustStore,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service
) : AzOptions;