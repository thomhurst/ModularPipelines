using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ad", "group", "get-member-groups")]
public record AzAdGroupGetMemberGroupsOptions(
[property: CliOption("--group")] string Group
) : AzOptions
{
    [CliFlag("--security-enabled-only")]
    public bool? SecurityEnabledOnly { get; set; }
}