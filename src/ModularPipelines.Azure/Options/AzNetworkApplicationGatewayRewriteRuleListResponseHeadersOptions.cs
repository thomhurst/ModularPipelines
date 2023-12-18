using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "rewrite-rule", "list-response-headers")]
public record AzNetworkApplicationGatewayRewriteRuleListResponseHeadersOptions : AzOptions;