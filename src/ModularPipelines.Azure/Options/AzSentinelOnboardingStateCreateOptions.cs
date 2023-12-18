using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sentinel", "onboarding-state", "create")]
public record AzSentinelOnboardingStateCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [BooleanCommandSwitch("--customer-managed-key")]
    public bool? CustomerManagedKey { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }
}