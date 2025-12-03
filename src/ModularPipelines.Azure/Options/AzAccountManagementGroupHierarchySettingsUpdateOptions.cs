using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("account", "management-group", "hierarchy-settings", "update")]
public record AzAccountManagementGroupHierarchySettingsUpdateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--default-management-group")]
    public string? DefaultManagementGroup { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--require-authorization-for-group-creation")]
    public string? RequireAuthorizationForGroupCreation { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }
}