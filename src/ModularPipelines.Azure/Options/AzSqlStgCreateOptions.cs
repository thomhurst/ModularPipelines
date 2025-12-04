using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "stg", "create")]
public record AzSqlStgCreateOptions(
[property: CliOption("--group-member")] string GroupMember,
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--trust-scope")] string TrustScope
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}