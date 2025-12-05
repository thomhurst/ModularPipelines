using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tasks", "describe")]
public record GcloudTasksDescribeOptions(
[property: CliArgument] string Task
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--queue")]
    public string? Queue { get; set; }

    [CliOption("--response-view")]
    public string? ResponseView { get; set; }
}