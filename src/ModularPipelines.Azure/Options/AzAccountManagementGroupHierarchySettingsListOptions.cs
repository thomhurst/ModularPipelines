using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "management-group", "hierarchy-settings", "list")]
public record AzAccountManagementGroupHierarchySettingsListOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;