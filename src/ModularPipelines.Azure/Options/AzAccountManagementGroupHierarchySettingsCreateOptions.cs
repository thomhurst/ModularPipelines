using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "management-group", "hierarchy-settings", "create")]
public record AzAccountManagementGroupHierarchySettingsCreateOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--default-management-group")]
    public string? DefaultManagementGroup { get; set; }

    [CommandSwitch("--require-authorization-for-group-creation")]
    public string? RequireAuthorizationForGroupCreation { get; set; }
}