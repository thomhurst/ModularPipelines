using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devops", "security", "group", "membership", "add")]
public record AzDevopsSecurityGroupMembershipAddOptions(
[property: CliOption("--group-id")] string GroupId,
[property: CliOption("--member-id")] string MemberId
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }
}