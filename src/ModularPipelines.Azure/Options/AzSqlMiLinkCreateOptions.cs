using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "mi", "link", "create")]
public record AzSqlMiLinkCreateOptions(
[property: CommandSwitch("--distributed-availability-group-name")] string DistributedAvailabilityGroupName,
[property: CommandSwitch("--instance-name")] string InstanceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--primary-ag")]
    public string? PrimaryAg { get; set; }

    [CommandSwitch("--secondary-ag")]
    public string? SecondaryAg { get; set; }

    [CommandSwitch("--source-endpoint")]
    public string? SourceEndpoint { get; set; }

    [CommandSwitch("--target-database")]
    public string? TargetDatabase { get; set; }
}