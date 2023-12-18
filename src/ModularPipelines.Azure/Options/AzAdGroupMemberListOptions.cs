using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "group", "member", "list")]
public record AzAdGroupMemberListOptions(
[property: CommandSwitch("--group")] string Group
) : AzOptions;