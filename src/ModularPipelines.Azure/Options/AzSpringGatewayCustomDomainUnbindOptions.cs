using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spring", "gateway", "custom-domain", "unbind")]
public record AzSpringGatewayCustomDomainUnbindOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions;