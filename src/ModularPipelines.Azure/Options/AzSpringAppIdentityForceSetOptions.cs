using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "app", "identity", "force-set")]
public record AzSpringAppIdentityForceSetOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service,
[property: CommandSwitch("--system-assigned")] string SystemAssigned,
[property: CommandSwitch("--user-assigned")] string UserAssigned
) : AzOptions
{
}

