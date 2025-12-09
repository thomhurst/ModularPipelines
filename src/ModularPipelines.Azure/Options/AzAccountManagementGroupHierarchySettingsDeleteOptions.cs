using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("account", "management-group", "hierarchy-settings", "delete")]
public record AzAccountManagementGroupHierarchySettingsDeleteOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}