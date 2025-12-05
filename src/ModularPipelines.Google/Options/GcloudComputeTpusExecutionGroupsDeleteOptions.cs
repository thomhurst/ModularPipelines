using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "tpus", "execution-groups", "delete")]
public record GcloudComputeTpusExecutionGroupsDeleteOptions(
[property: CliArgument] string ExecutionGroupName
) : GcloudOptions
{
    [CliFlag("--tpu-only")]
    public bool? TpuOnly { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}