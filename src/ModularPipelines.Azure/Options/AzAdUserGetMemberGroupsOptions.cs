using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ad", "user", "get-member-groups")]
public record AzAdUserGetMemberGroupsOptions(
[property: CliOption("--id")] string Id
) : AzOptions
{
    [CliFlag("--security-enabled-only")]
    public bool? SecurityEnabledOnly { get; set; }
}