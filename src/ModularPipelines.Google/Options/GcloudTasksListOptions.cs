using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tasks", "list")]
public record GcloudTasksListOptions(
[property: CliOption("--queue")] string Queue
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}