using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("identity", "groups", "create")]
public record GcloudIdentityGroupsCreateOptions(
[property: PositionalArgument] string Email,
[property: CommandSwitch("--customer")] string Customer,
[property: CommandSwitch("--organization")] string Organization
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--dynamic-user-query")]
    public string? DynamicUserQuery { get; set; }

    [CommandSwitch("--with-initial-owner")]
    public string? WithInitialOwner { get; set; }

    [CommandSwitch("--group-type")]
    public string? GroupType { get; set; }

    [BooleanCommandSwitch("discussion")]
    public bool? Discussion { get; set; }

    [BooleanCommandSwitch("dynamic")]
    public bool? Dynamic { get; set; }

    [BooleanCommandSwitch("security")]
    public bool? Security { get; set; }

    [CommandSwitch("--labels")]
    public string? Labels { get; set; }
}