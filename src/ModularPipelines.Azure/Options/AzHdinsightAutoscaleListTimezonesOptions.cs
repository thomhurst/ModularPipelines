using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("hdinsight", "autoscale", "list-timezones")]
public record AzHdinsightAutoscaleListTimezonesOptions : AzOptions;