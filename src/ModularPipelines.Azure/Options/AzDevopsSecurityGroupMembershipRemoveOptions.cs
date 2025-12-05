using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devops", "security", "group", "membership", "remove")]
public record AzDevopsSecurityGroupMembershipRemoveOptions(
[property: CliOption("--group-id")] string GroupId,
[property: CliOption("--member-id")] string MemberId
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}