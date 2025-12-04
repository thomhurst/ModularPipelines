using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dt", "role-assignment", "create")]
public record AzDtRoleAssignmentCreateOptions(
[property: CliOption("--assignee")] string Assignee,
[property: CliOption("--dt-name")] string DtName,
[property: CliOption("--role")] string Role
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}