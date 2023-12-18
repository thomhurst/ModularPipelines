using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "group", "member", "remove")]
public record AzAdGroupMemberRemoveOptions(
[property: CommandSwitch("--group")] string Group,
[property: CommandSwitch("--member-id")] string MemberId
) : AzOptions
{
}