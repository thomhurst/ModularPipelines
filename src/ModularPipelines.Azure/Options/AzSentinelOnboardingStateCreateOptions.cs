using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sentinel", "onboarding-state", "create")]
public record AzSentinelOnboardingStateCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliFlag("--customer-managed-key")]
    public bool? CustomerManagedKey { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }
}