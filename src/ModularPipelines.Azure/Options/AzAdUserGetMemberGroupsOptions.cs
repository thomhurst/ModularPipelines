using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "user", "get-member-groups")]
public record AzAdUserGetMemberGroupsOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions
{
    [BooleanCommandSwitch("--security-enabled-only")]
    public bool? SecurityEnabledOnly { get; set; }
}