using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "instances", "tables", "describe")]
public record GcloudBigtableInstancesTablesDescribeOptions(
[property: CliArgument] string Table,
[property: CliArgument] string Instance
) : GcloudOptions
{
    [CliOption("--view")]
    public string? View { get; set; }
}