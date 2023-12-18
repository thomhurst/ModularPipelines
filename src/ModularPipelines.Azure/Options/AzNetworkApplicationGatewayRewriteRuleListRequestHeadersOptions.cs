using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "rewrite-rule", "list-request-headers")]
public record AzNetworkApplicationGatewayRewriteRuleListRequestHeadersOptions : AzOptions;