using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("identity", "groups", "update")]
public record GcloudIdentityGroupsUpdateOptions(
[property: PositionalArgument] string Email
) : GcloudOptions
{
    [CommandSwitch("--dynamic-user-query")]
    public string? DynamicUserQuery { get; set; }

    [CommandSwitch("--labels")]
    public string? Labels { get; set; }

    [BooleanCommandSwitch("--clear-description")]
    public bool? ClearDescription { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--clear-display-name")]
    public bool? ClearDisplayName { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }
}