using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mobile-network", "service", "create")]
public record AzMobileNetworkServiceCreateOptions(
[property: CliOption("--mobile-network-name")] string MobileNetworkName,
[property: CliOption("--name")] string Name,
[property: CliOption("--pcc-rules")] string PccRules,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-precedence")] string ServicePrecedence
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--service-qos-policy")]
    public string? ServiceQosPolicy { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}