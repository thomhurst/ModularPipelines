using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stack-hci", "cluster", "extend-software-assurance-benefit")]
public record AzStackHciClusterExtendSoftwareAssuranceBenefitOptions : AzOptions
{
    [CommandSwitch("--cluster-name")]
    public string? ClusterName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--software-assurance-intent")]
    public string? SoftwareAssuranceIntent { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}