using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "application-gateway", "waf-policy", "policy-setting", "list")]
public record AzNetworkApplicationGatewayWafPolicyPolicySettingListOptions(
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;