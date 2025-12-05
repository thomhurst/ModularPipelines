using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "instance-failover-group", "create")]
public record AzSqlInstanceFailoverGroupCreateOptions(
[property: CliOption("--mi")] string Mi,
[property: CliOption("--name")] string Name,
[property: CliOption("--partner-mi")] string PartnerMi,
[property: CliOption("--partner-resource-group")] string PartnerResourceGroup,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--failover-policy")]
    public string? FailoverPolicy { get; set; }

    [CliOption("--grace-period")]
    public string? GracePeriod { get; set; }

    [CliOption("--secondary-type")]
    public string? SecondaryType { get; set; }
}