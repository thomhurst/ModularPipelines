using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("disk", "grant-access")]
public record AzDiskGrantAccessOptions(
[property: CommandSwitch("--duration-in-seconds")] string DurationInSeconds
) : AzOptions
{
    [CommandSwitch("--access-level")]
    public string? AccessLevel { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--secure-vm-guest-state-sas")]
    public string? SecureVmGuestStateSas { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}