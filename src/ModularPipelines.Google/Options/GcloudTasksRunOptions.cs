using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tasks", "run")]
public record GcloudTasksRunOptions(
[property: CliArgument] string Task
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--queue")]
    public string? Queue { get; set; }
}