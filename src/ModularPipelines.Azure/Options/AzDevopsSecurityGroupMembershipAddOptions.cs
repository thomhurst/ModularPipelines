using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "security", "group", "membership", "add")]
public record AzDevopsSecurityGroupMembershipAddOptions(
[property: CommandSwitch("--group-id")] string GroupId,
[property: CommandSwitch("--member-id")] string MemberId
) : AzOptions
{
    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }
}