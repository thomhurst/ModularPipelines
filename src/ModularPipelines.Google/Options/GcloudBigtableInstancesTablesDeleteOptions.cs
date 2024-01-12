using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "instances", "tables", "delete")]
public record GcloudBigtableInstancesTablesDeleteOptions(
[property: PositionalArgument] string Table,
[property: PositionalArgument] string Instance
) : GcloudOptions;