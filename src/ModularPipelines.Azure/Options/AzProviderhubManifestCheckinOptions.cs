using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("providerhub", "manifest", "checkin")]
public record AzProviderhubManifestCheckinOptions(
[property: CliOption("--arm-manifest-location")] string ArmManifestLocation,
[property: CliOption("--environment")] string Environment
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--provider-namespace")]
    public string? ProviderNamespace { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}