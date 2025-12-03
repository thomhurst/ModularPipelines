using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "instances", "tables", "list")]
public record GcloudBigtableInstancesTablesListOptions(
[property: CliOption("--instances")] string[] Instances
) : GcloudOptions;