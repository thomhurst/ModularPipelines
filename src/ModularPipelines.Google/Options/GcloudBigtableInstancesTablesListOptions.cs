using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "instances", "tables", "list")]
public record GcloudBigtableInstancesTablesListOptions(
[property: CommandSwitch("--instances")] string[] Instances
) : GcloudOptions;