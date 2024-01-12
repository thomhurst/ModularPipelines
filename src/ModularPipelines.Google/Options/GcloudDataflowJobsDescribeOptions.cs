using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataflow", "jobs", "describe")]
public record GcloudDataflowJobsDescribeOptions(
[property: PositionalArgument] string JobId
) : GcloudOptions
{
    [BooleanCommandSwitch("--full")]
    public bool? Full { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}