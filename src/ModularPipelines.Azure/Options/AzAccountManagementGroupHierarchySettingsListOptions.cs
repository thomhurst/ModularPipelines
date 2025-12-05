using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("account", "management-group", "hierarchy-settings", "list")]
public record AzAccountManagementGroupHierarchySettingsListOptions(
[property: CliOption("--name")] string Name
) : AzOptions;