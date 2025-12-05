using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("hdinsight", "autoscale", "list-timezones")]
public record AzHdinsightAutoscaleListTimezonesOptions : AzOptions;