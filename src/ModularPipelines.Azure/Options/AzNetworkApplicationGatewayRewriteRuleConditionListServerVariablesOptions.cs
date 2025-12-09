using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "application-gateway", "rewrite-rule", "condition", "list-server-variables")]
public record AzNetworkApplicationGatewayRewriteRuleConditionListServerVariablesOptions : AzOptions;