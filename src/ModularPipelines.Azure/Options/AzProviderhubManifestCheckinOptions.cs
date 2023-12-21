using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("providerhub", "manifest", "checkin")]
public record AzProviderhubManifestCheckinOptions(
[property: CommandSwitch("--arm-manifest-location")] string ArmManifestLocation,
[property: CommandSwitch("--environment")] string Environment
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--provider-namespace")]
    public string? ProviderNamespace { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}