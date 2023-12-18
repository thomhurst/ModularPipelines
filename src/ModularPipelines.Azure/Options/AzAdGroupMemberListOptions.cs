using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "group", "member", "list")]
public record AzAdGroupMemberListOptions(
[property: CommandSwitch("--group")] string Group
) : AzOptions
{
}