using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "management-group", "hierarchy-settings", "list")]
public record AzAccountManagementGroupHierarchySettingsListOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}