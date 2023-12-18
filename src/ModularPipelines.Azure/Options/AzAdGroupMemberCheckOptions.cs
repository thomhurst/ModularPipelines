using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "group", "member", "check")]
public record AzAdGroupMemberCheckOptions(
[property: CommandSwitch("--group")] string Group,
[property: CommandSwitch("--member-id")] string MemberId
) : AzOptions
{
}