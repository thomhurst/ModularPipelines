using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "instances", "tables", "delete")]
public record GcloudBigtableInstancesTablesDeleteOptions(
[property: CliArgument] string Table,
[property: CliArgument] string Instance
) : GcloudOptions;