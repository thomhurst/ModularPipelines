using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "mi", "link", "create")]
public record AzSqlMiLinkCreateOptions(
[property: CliOption("--distributed-availability-group-name")] string DistributedAvailabilityGroupName,
[property: CliOption("--instance-name")] string InstanceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--primary-ag")]
    public string? PrimaryAg { get; set; }

    [CliOption("--secondary-ag")]
    public string? SecondaryAg { get; set; }

    [CliOption("--source-endpoint")]
    public string? SourceEndpoint { get; set; }

    [CliOption("--target-database")]
    public string? TargetDatabase { get; set; }
}