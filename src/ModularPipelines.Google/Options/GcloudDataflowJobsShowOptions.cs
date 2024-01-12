using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataflow", "jobs", "show")]
public record GcloudDataflowJobsShowOptions(
[property: PositionalArgument] string JobId
) : GcloudOptions
{
    [BooleanCommandSwitch("--environment")]
    public bool? Environment { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [BooleanCommandSwitch("--steps")]
    public bool? Steps { get; set; }
}