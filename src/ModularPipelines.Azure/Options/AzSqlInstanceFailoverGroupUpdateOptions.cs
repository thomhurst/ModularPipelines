using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "instance-failover-group", "update")]
public record AzSqlInstanceFailoverGroupUpdateOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--failover-policy")]
    public string? FailoverPolicy { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--grace-period")]
    public string? GracePeriod { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--secondary-type")]
    public string? SecondaryType { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }
}