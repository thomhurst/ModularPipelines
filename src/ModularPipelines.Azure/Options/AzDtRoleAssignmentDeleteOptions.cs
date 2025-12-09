using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dt", "role-assignment", "delete")]
public record AzDtRoleAssignmentDeleteOptions(
[property: CliOption("--dt-name")] string DtName
) : AzOptions
{
    [CliOption("--assignee")]
    public string? Assignee { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }
}