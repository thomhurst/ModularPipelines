using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("role", "assignment", "list")]
public record AzRoleAssignmentListOptions : AzOptions
{
    [CliFlag("--all")]
    public bool? All { get; set; }

    [CliOption("--assignee")]
    public string? Assignee { get; set; }

    [CliFlag("--include-classic-administrators")]
    public bool? IncludeClassicAdministrators { get; set; }

    [CliFlag("--include-groups")]
    public bool? IncludeGroups { get; set; }

    [CliFlag("--include-inherited")]
    public bool? IncludeInherited { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }
}