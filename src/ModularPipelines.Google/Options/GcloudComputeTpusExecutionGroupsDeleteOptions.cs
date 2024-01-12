using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "tpus", "execution-groups", "delete")]
public record GcloudComputeTpusExecutionGroupsDeleteOptions(
[property: PositionalArgument] string ExecutionGroupName
) : GcloudOptions
{
    [BooleanCommandSwitch("--tpu-only")]
    public bool? TpuOnly { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}