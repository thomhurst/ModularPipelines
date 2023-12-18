using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("relay", "wcfrelay", "create")]
public record AzRelayWcfrelayCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--relay-type")]
    public string? RelayType { get; set; }

    [BooleanCommandSwitch("--requires-client-authorization")]
    public bool? RequiresClientAuthorization { get; set; }

    [BooleanCommandSwitch("--requires-transport-security")]
    public bool? RequiresTransportSecurity { get; set; }

    [CommandSwitch("--user-metadata")]
    public string? UserMetadata { get; set; }
}