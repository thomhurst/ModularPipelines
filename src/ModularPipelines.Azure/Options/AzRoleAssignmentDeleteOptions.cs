using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("role", "assignment", "delete")]
public record AzRoleAssignmentDeleteOptions : AzOptions
{
    [CliOption("--assignee")]
    public string? Assignee { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--include-inherited")]
    public bool? IncludeInherited { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--yes")]
    public bool? Yes { get; set; } = true;
}