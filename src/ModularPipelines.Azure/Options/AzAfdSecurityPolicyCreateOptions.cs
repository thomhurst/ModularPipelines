using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("afd", "security-policy", "create")]
public record AzAfdSecurityPolicyCreateOptions(
[property: CommandSwitch("--domains")] string Domains,
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--security-policy-name")] string SecurityPolicyName,
[property: CommandSwitch("--waf-policy")] string WafPolicy
) : AzOptions
{
}