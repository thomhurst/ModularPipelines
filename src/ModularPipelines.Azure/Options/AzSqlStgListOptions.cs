using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "stg", "list")]
public record AzSqlStgListOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--instance-name")]
    public string? InstanceName { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}