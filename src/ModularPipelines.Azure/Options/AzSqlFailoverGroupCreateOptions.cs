using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "failover-group", "create")]
public record AzSqlFailoverGroupCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--partner-server")] string PartnerServer,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--server")] string Server
) : AzOptions
{
    [CliOption("--add-db")]
    public string? AddDb { get; set; }

    [CliOption("--failover-policy")]
    public string? FailoverPolicy { get; set; }

    [CliOption("--grace-period")]
    public string? GracePeriod { get; set; }

    [CliOption("--partner-resource-group")]
    public string? PartnerResourceGroup { get; set; }
}