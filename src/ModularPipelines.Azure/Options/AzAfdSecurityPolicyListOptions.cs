using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("afd", "security-policy", "list")]
public record AzAfdSecurityPolicyListOptions(
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;