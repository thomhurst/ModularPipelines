using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "instances", "tables", "describe")]
public record GcloudBigtableInstancesTablesDescribeOptions(
[property: PositionalArgument] string Table,
[property: PositionalArgument] string Instance
) : GcloudOptions
{
    [CommandSwitch("--view")]
    public string? View { get; set; }
}