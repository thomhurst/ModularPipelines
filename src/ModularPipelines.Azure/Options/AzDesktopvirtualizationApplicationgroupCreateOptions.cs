using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("desktopvirtualization", "applicationgroup", "create")]
public record AzDesktopvirtualizationApplicationgroupCreateOptions(
[property: CommandSwitch("--application-group-type")] string ApplicationGroupType,
[property: CommandSwitch("--host-pool-arm-path")] string HostPoolArmPath,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--friendly-name")]
    public string? FriendlyName { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}