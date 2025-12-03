using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "application-gateway", "rewrite-rule", "condition", "list-server-variables")]
public record AzNetworkApplicationGatewayRewriteRuleConditionListServerVariablesOptions : AzOptions;