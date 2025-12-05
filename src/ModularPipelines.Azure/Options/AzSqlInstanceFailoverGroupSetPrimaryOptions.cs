using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "instance-failover-group", "set-primary")]
public record AzSqlInstanceFailoverGroupSetPrimaryOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--allow-data-loss")]
    public bool? AllowDataLoss { get; set; }
}