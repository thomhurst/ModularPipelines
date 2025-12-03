using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "application-gateway", "rewrite-rule", "list-response-headers")]
public record AzNetworkApplicationGatewayRewriteRuleListResponseHeadersOptions : AzOptions;