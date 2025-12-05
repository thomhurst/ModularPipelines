using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dt", "role-assignment", "list")]
public record AzDtRoleAssignmentListOptions(
[property: CliOption("--dt-name")] string DtName
) : AzOptions
{
    [CliFlag("--include-inherited")]
    public bool? IncludeInherited { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }
}