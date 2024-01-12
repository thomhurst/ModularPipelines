using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataflow", "jobs", "cancel")]
public record GcloudDataflowJobsCancelOptions(
[property: PositionalArgument] string JobId
) : GcloudOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}