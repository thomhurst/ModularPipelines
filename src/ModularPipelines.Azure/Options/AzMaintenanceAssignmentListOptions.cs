using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("maintenance", "assignment", "list")]
public record AzMaintenanceAssignmentListOptions(
[property: CommandSwitch("--provider-name")] string ProviderName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-name")] string ResourceName,
[property: CommandSwitch("--resource-type")] string ResourceType
) : AzOptions;