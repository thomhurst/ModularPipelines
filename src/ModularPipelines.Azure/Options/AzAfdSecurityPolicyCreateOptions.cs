using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("afd", "security-policy", "create")]
public record AzAfdSecurityPolicyCreateOptions(
[property: CliOption("--domains")] string Domains,
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--security-policy-name")] string SecurityPolicyName,
[property: CliOption("--waf-policy")] string WafPolicy
) : AzOptions;