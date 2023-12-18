using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "group", "get-member-groups")]
public record AzAdGroupGetMemberGroupsOptions(
[property: CommandSwitch("--group")] string Group
) : AzOptions
{
    [BooleanCommandSwitch("--security-enabled-only")]
    public bool? SecurityEnabledOnly { get; set; }
}