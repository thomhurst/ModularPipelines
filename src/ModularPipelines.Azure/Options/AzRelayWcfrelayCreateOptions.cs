using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("relay", "wcfrelay", "create")]
public record AzRelayWcfrelayCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--relay-type")]
    public string? RelayType { get; set; }

    [CliFlag("--requires-client-authorization")]
    public bool? RequiresClientAuthorization { get; set; }

    [CliFlag("--requires-transport-security")]
    public bool? RequiresTransportSecurity { get; set; }

    [CliOption("--user-metadata")]
    public string? UserMetadata { get; set; }
}