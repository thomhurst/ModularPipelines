using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "app", "identity", "force-set")]
public record AzSpringAppIdentityForceSetOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service,
[property: CommandSwitch("--system-assigned")] string SystemAssigned,
[property: CommandSwitch("--user-assigned")] string UserAssigned
) : AzOptions;