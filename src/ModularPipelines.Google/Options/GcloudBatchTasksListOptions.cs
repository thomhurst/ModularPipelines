using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "tasks", "list")]
public record GcloudBatchTasksListOptions(
[property: CliOption("--job")] string Job,
[property: CliOption("--location")] string Location
) : GcloudOptions;