using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataflow", "jobs", "update-options")]
public record GcloudDataflowJobsUpdateOptionsOptions(
[property: PositionalArgument] string JobId
) : GcloudOptions
{
    [CommandSwitch("--max-num-workers")]
    public string? MaxNumWorkers { get; set; }

    [CommandSwitch("--min-num-workers")]
    public string? MinNumWorkers { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}