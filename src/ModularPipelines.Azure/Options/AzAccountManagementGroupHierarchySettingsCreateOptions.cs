using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("account", "management-group", "hierarchy-settings", "create")]
public record AzAccountManagementGroupHierarchySettingsCreateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--default-management-group")]
    public string? DefaultManagementGroup { get; set; }

    [CliOption("--require-authorization-for-group-creation")]
    public string? RequireAuthorizationForGroupCreation { get; set; }
}